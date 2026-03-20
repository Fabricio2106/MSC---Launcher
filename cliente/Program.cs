using cliente.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ClienteDbContext>(options =>
    options.UseInMemoryDatabase("ClienteDb"));

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();