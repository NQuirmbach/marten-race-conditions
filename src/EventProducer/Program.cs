using EventProducer.Messages;
using Oakton;
using ServiceDefaults;
using ServiceDefaults.Messages;
using Wolverine;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    // Delay for dependencies to start properly
    await Task.Delay(5000);
}

builder.AddServiceDefaults();
builder.UseWolverineDefaults(typeof(Program).Assembly, options =>
{
    options.PublishMessage<UserCreated>().ToRabbitQueue(Const.Queues.UserCreated);
    options.PublishMessage<TaskCreated>().ToRabbitQueue(Const.Queues.TaskCreated);
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGet("/", () => "Hello World!");
app.MapGet("/generate", (IMessageBus bus) => bus.PublishAsync(new GenerateEvents()));

// Opt into using Oakton for command line parsing
// to unlock built in diagnostics and utility tools within
// your Wolverine application
return await app.RunOaktonCommands(args);