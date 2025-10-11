using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddAPIServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseAPIServices();

if (app.Environment.IsDevelopment())
    await app.InitializeDatabaseAsync();

app.Run();
