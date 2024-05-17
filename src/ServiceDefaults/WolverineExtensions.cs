using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Oakton.Resources;
using RabbitMQ.Client;
using ServiceDefaults;
using ServiceDefaults.Messages;
using Wolverine;
using Wolverine.RabbitMQ;

namespace Microsoft.Extensions.Hosting;

public static class WolverineExtensions
{
    public static void UseWolverineDefaults(this WebApplicationBuilder builder, Assembly applicationAssembly, Action<WolverineOptions>? setup = null)
    {
        builder.Services.AddResourceSetupOnStartup();
        
        builder.Host.UseWolverine(options =>
        {
            options.ApplicationAssembly = applicationAssembly;
            
            var connectionString = builder.Configuration.GetConnectionString(Const.ConnectionStrings.RabbitMQ);
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                options.UseRabbitMq(new Uri(connectionString!))
                    .AutoProvision();
            }

            options.Services.AddOpenTelemetry();
            
            setup?.Invoke(options);
        });
    }

    public static async Task WaitForRabbitMq(this WebApplicationBuilder builder)
    {
        var logger = builder.Services
            .BuildServiceProvider()
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger(nameof(WolverineExtensions));
        
        var connectionString = builder.Configuration.GetConnectionString(Const.ConnectionStrings.RabbitMQ);
        var factory = new ConnectionFactory
        {
            Uri = new Uri(connectionString!)
        };
        var connected = false;
        const int maxRetries = 5;
        const int delay = 2;

        var retries = 0;
        
        logger.LogInformation("Waiting for RabbitMQ...");

        while (!connected && retries < maxRetries)
        {
            try
            {
                using var connection = factory.CreateConnection();
                connected = true;
            }
            catch
            {
                logger.LogInformation("RabbitMQ connection not successful, delaying...");
                retries++;
                await Task.Delay(delay * 1000);
            }
        }

        if (retries == maxRetries)
            throw new Exception("Cannot connect to RabbitMQ");
        
        logger.LogInformation("Successfully connected to RabbitMQ");
    }
}