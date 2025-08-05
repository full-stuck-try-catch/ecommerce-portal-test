using System.Reflection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PixelzEcommerce.Api.Extensions;
using PixelzEcommerce.Api.Middleware;
using PixelzEcommerce.Modules.Orders.Infrastructure;
using PixelzEcommerce.Shared.Application;
using PixelzEcommerce.Shared.Infrastructure;
using PixelzEcommerce.Shared.Infrastructure.Configuration;
using PixelzEcommerce.Shared.Presentation.Endpoints;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

Assembly[] moduleApplicationAssemblies = [
    PixelzEcommerce.Modules.Products.Application.AssemblyReference.Assembly,
    PixelzEcommerce.Modules.Products.Application.AssemblyReference.Assembly];

builder.Services.AddApplication(moduleApplicationAssemblies);

string databaseConnectionString = builder.Configuration.GetConnectionStringOrThrow("Database");
string redisConnectionString = builder.Configuration.GetConnectionStringOrThrow("Cache");


builder.Services.AddHealthChecks()
    .AddNpgSql(databaseConnectionString)
    .AddRedis(redisConnectionString);

builder.Configuration.AddModuleConfiguration(["orders", "products"]);

builder.Services.AddOrderModule(builder.Configuration);

builder.Services.AddInfrastructure(
    [
        OrderModule.ConfigureConsumers
    ],
    databaseConnectionString,
    redisConnectionString);
// Configure the HTTP request pipeline.

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();

    app.SeedData();
}

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseLogContext();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.MapEndpoints();

app.Run();

internal partial class Program;
