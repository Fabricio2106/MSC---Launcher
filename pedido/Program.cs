using pedido.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PedidoDbContext>(options =>
    options.UseInMemoryDatabase("PedidoDb"));

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
