using Microsoft.EntityFrameworkCore;
using Universal.UsersService.Api.Domain.Entities;
using Universal.UsersService.Api.Infrastructure.Security;
using FluentValidation;
using MediatR;
using Polly.Extensions.Http;
var builder = WebApplication.CreateBuilder(args);

// 1. Registrar la clase de configuración
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
// Registrar configuración fuertemente tipada para JWT
builder.Services.Configure<Universal.UsersService.Api.Infrastructure.Security.JwtSettings>(
    builder.Configuration.GetSection(Universal.UsersService.Api.Infrastructure.Security.JwtSettings.SectionName));

// Add services to the container.


// Registrar FluentValidation y los validadores
builder.Services.AddValidatorsFromAssembly(typeof(Universal.UsersService.Api.Application.Commands.RegisterUserCommand).Assembly);

// Registrar Pipeline Behavior para validación con MediatR
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Universal.UsersService.Api.Application.Behaviors.ValidationBehavior<,>));

// Registro de MediatR para CQRS
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Universal.UsersService.Api.Application.Commands.RegisterUserCommand).Assembly));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Leer el nombre de la base de datos desde la configuración
var dbName = builder.Configuration["InMemoryDatabaseName"] ?? "UserDb";
builder.Services.AddDbContext<Universal.UsersService.Api.Infrastructure.Persistence.AppDbContext>(options =>
    options.UseInMemoryDatabase(dbName));

//Usar mismo tipo de DI en este caso.
//Scoped para servicios de aplicación y repositorios es el estándar de oro en ASP.NET Core, especialmente porque el DbContext también es Scoped por defecto
builder.Services.AddScoped<Universal.UsersService.Api.Domain.Repositories.IUserRepository, Universal.UsersService.Api.Infrastructure.Persistence.UserRepository>();

// Registro del servicio de aplicación para usuarios


// Configurar HttpClientFactory con Polly para resiliencia y timeout
builder.Services.AddHttpClient<Universal.UsersService.Api.Application.Gateways.IExternalPostGateway, Universal.UsersService.Api.Infrastructure.Gateways.ExternalPostGateway>()
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
    .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, retry => TimeSpan.FromSeconds(Math.Pow(2, retry))))
    .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(2, TimeSpan.FromSeconds(30)))
    .ConfigureHttpClient(client =>
    {
        client.Timeout = TimeSpan.FromSeconds(10); // Timeout estricto
    });



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

app.UseAuthorization();

app.MapControllers();

app.Run();
