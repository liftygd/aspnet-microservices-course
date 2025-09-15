var builder = WebApplication.CreateBuilder(args);

//Services
builder.Services.AddCarter();
builder.Services.AddMediatR(config => 
{ 
    config.RegisterServicesFromAssembly(typeof(Program).Assembly); 
});
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

var app = builder.Build();

//Middleware
app.MapCarter();

app.Run();
