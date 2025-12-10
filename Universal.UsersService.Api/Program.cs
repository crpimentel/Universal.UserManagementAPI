using Microsoft.EntityFrameworkCore;
using Universal.UsersService.Api.Domain.Entities;
using Universal.UsersService.Api.Infrastructure.Security;
var builder = WebApplication.CreateBuilder(args);

// 1. Registrar la clase de configuración
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
// Registrar configuración fuertemente tipada para JWT
builder.Services.Configure<Universal.UsersService.Api.Infrastructure.Security.JwtSettings>(
    builder.Configuration.GetSection(Universal.UsersService.Api.Infrastructure.Security.JwtSettings.SectionName));

// Add services to the container.

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

// Registro del servicio para generación de tokens
builder.Services.AddScoped<Universal.UsersService.Api.Infrastructure.Security.ITokenService, Universal.UsersService.Api.Infrastructure.Security.JwtTokenService>();


var app = builder.Build();

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
