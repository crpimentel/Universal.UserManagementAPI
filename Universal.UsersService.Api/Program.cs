using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Leer el nombre de la base de datos desde la configuración
var dbName = builder.Configuration["InMemoryDatabaseName"] ?? "UserDb";
builder.Services.AddDbContext<Universal.UsersService.Api.Infrastructure.Persistence.AppDbContext>(options =>
    options.UseInMemoryDatabase(dbName));
builder.Services.AddScoped<Universal.UsersService.Api.Domain.Repositories.IUserRepository, Universal.UsersService.Api.Infrastructure.Persistence.UserRepository>();


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
