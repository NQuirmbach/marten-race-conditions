using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oakton.Resources;
using ServiceDefaults;
using ServiceDefaults.Messages;
using Wolverine;
using Wolverine.RabbitMQ;

namespace Microsoft.Extensions.Hosting;

public static class WolverineExtensions
{
    public static void UseWolverineDefaults(this WebApplicationBuilder builder, Assembly applicationAssembly)
    {
        builder.Services.AddResourceSetupOnStartup();
        
        builder.Host.UseWolverine(options =>
        {
            options.ApplicationAssembly = applicationAssembly;
            
            var connectionString = builder.Configuration.GetConnectionString(Const.ConnectionStrings.RabbitMQ);
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                options.UseRabbitMq(new Uri(connectionString))
                    .AutoProvision();
                
                options.PublishMessage<UserCreated>().ToRabbitQueue("user-created");
                options.PublishMessage<TaskCreated>().ToRabbitQueue("task-created");
            }

            options.Services.AddOpenTelemetry();
        });
    } 
}