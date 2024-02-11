using Microsoft.OpenApi.Models;
using OperacionFuegoQuasar.Application.Services;
using OperacionFuegoQuasar.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IShipService, ShipService>();
// Configurar Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OperacionFuegoQuasar API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "OperacionFuegoQuasar API v1");
    c.RoutePrefix = string.Empty;
});

app.Run();
