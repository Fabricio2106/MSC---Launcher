using cliente.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Agregar controladores
builder.Services.AddControllers();

// Configurar Entity Framework Core
var connectionString = builder.Configuration.GetConnectionString("ClienteConnection") 
    ?? "Server=.;Database=ClienteDb;Trusted_Connection=true;TrustServerCertificate=true;";

builder.Services.AddDbContext<ClienteDbContext>(options =>
    options.UseSqlServer(connectionString));

// Agregar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Usar CORS
app.UseCors("AllowAll");

// Usar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

app.Run();

