using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OperacionFuegoQuasar.Application.Services;
using OperacionFuegoQuasar.Infrastructure.Data;
using OperacionFuegoQuasar.Domain.Repositories;
using OperacionFuegoQuasar.Infrastructure.Repositories;
using OperacionFuegoQuasar.Aplication.Services;
using OperacionFuegoQuasar.Application.Exceptions;
using System.Net;
using OperacionFuegoQuasar.Infrastructure.Exceptions;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddScoped<IShipService, ShipService>();
// Configurar Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OperacionFuegoQuasar API", Version = "v1" });
});

var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlite(connectionString));

builder.Services.AddScoped<ISatelliteDataRepository, SatelliteDataRepository>();

var app = builder.Build();


app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";

        var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        if (exception is UserException || exception is InfrastructureException)
        {
            var response = new { message = exception.Message };
            var jsonResponse = JsonConvert.SerializeObject(response);
            await context.Response.WriteAsync(jsonResponse);
        }
        else
        {
            await context.Response.WriteAsync("Internal Server Error");
        }
    });
});

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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
