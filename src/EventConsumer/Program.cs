using Oakton;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.UseWolverineDefaults(typeof(Program).Assembly, options =>
{
    
});

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapGet("/", () => "Hello World!");

// Opt into using Oakton for command line parsing
// to unlock built in diagnostics and utility tools within
// your Wolverine application
return await app.RunOaktonCommands(args);