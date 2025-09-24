using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.Behaviours;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using FluentValidation;
using HealthChecks.UI.Client;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

var dbConnection = builder.Configuration.GetConnectionString("Database");
var redisConnection = builder.Configuration.GetConnectionString("Redis");

//Services
builder.Services.AddCarter();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(dbConnection!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddStackExchangeRedisCache(opts =>
{
    opts.Configuration = redisConnection;
});

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(dbConnection!)
    .AddRedis(redisConnection!);

var app = builder.Build();

//Middleware
app.MapCarter();
app.UseExceptionHandler(opts => { });
app.UseHealthChecks("/health", 
    new HealthCheckOptions 
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    });

app.Run();
