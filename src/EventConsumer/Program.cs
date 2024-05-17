using EventConsumer.Persistence;
using Oakton;
using ServiceDefaults;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    // Delay for dependencies to start properly
    await builder.WaitForRabbitMq();
}

builder.AddServiceDefaults();
builder.AddMartenDefaults();
builder.UseWolverineDefaults(typeof(Program).Assembly, options =>
{
    const int listeners = 2;
    options.ListenToRabbitQueue(Const.Queues.UserCreated)
        .ListenerCount(listeners);
    options.ListenToRabbitQueue(Const.Queues.TaskCreated)
        .ListenerCount(listeners);
});

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapGet("/", () => "Hello World!");

// Opt into using Oakton for command line parsing
// to unlock built in diagnostics and utility tools within
// your Wolverine application
return await app.RunOaktonCommands(args);