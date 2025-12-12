using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using System.Text;
using Universal.UsersService.Api.Infrastructure.Security;
var builder = WebApplication.CreateBuilder(args);


// Registrar configuración fuertemente tipada para JWT
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(JwtSettings.SectionName));

// --- PASO CLAVE: OBTENER LA CONFIGURACIÓN ANTES DE USARLA EN EL SERVICIO ---
var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
    ?? throw new InvalidOperationException($"La sección {nameof(JwtSettings)} no fue configurada.");

// 1. Configurar Autenticación
builder.Services.AddAuthentication(options =>
{
    // Establecer el esquema JWT Bearer como el esquema por defecto.
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// 2. Configurar el JWT Bearer
.AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         // Validación de Clave
         ValidateIssuerSigningKey = true,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key
             ?? throw new InvalidOperationException("JWT Key not configured."))),

         // Validación de Emisor (Issuer)
         ValidateIssuer = true,
         ValidIssuer = jwtSettings.Issuer,

         // Validación de Audiencia (Audience)
         ValidateAudience = true,
         ValidAudience = jwtSettings.Audience,

         // Controlar el tiempo de vida (expiración)
         ValidateLifetime = true,
         ClockSkew = TimeSpan.Zero // Recomendado para eliminar la tolerancia de 5 minutos
     };
 });
// Registrar FluentValidation y los validadores
builder.Services.AddValidatorsFromAssembly(typeof(Universal.UsersService.Api.Application.Commands.RegisterUserCommand).Assembly);

// Registrar Pipeline Behavior para validación con MediatR
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Universal.UsersService.Api.Application.Behaviors.ValidationBehavior<,>));

// Registro de MediatR para CQRS
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Universal.UsersService.Api.Application.Commands.RegisterUserCommand).Assembly));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Universal Users Service API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingresa el token JWT en el formato 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


// Leer el nombre de la base de datos desde la configuración
var dbName = builder.Configuration["InMemoryDatabaseName"] ?? "UserDb";
builder.Services.AddDbContext<Universal.UsersService.Api.Infrastructure.Persistence.AppDbContext>(options =>
    options.UseInMemoryDatabase(dbName));

//Usar mismo tipo de DI en este caso.
//Scoped para servicios de aplicación y repositorios es el estándar de oro en ASP.NET Core, especialmente porque el DbContext también es Scoped por defecto
builder.Services.AddScoped<Universal.UsersService.Api.Domain.Repositories.IUserRepository, Universal.UsersService.Api.Infrastructure.Persistence.UserRepository>();

// Registro del servicio de aplicación para usuarios

// Registro del servicio para generación de tokens
builder.Services.AddTransient<Universal.UsersService.Api.Infrastructure.Security.ITokenService, Universal.UsersService.Api.Infrastructure.Security.JwtTokenService>();

// Configurar HttpClientFactory con Polly para resiliencia y timeout
builder.Services.AddHttpClient<Universal.UsersService.Api.Application.Gateways.IExternalPostGateway, Universal.UsersService.Api.Infrastructure.Gateways.ExternalPostGateway>(
    (serviceProvider, client) =>
    {
        var config = serviceProvider.GetRequiredService<IConfiguration>();
        client.BaseAddress = new Uri(config["ExternalApi:BaseUrl"] ?? "https://jsonplaceholder.typicode.com");
        client.Timeout = TimeSpan.FromSeconds(10); // Timeout estricto
    })
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
    .AddPolicyHandler(Polly.Extensions.Http.HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retry => TimeSpan.FromSeconds(Math.Pow(2, retry))))
    .AddPolicyHandler(Polly.Extensions.Http.HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30)));



var app = builder.Build();

// Middleware de manejo global de excepciones
app.UseMiddleware<Universal.UsersService.Api.API.Middleware.GlobalExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
