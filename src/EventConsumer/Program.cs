using EventConsumer.Persistence;
using Oakton;
using ServiceDefaults;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    // Delay for dependencies to start properly
    await Task.Delay(5000);
}

builder.AddServiceDefaults();
builder.AddMartenDefaults();
builder.UseWolverineDefaults(typeof(Program).Assembly, options =>
{
    options.ListenToRabbitQueue(Const.Queues.UserCreated);
    options.ListenToRabbitQueue(Const.Queues.TaskCreated);
});

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapGet("/", () => "Hello World!");

// Opt into using Oakton for command line parsing
// to unlock built in diagnostics and utility tools within
// your Wolverine application
return await app.RunOaktonCommands(args);