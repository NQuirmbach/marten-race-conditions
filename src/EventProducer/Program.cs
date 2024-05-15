using EventProducer.Messages;
using Oakton;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.UseWolverineDefaults(typeof(Program).Assembly);

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGet("/", () => "Hello World!");
app.MapGet("/generate", (IMessageBus bus) => bus.PublishAsync(new GenerateEvents()));

// Opt into using Oakton for command line parsing
// to unlock built in diagnostics and utility tools within
// your Wolverine application
return await app.RunOaktonCommands(args);