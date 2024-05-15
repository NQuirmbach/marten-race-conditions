using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly;
using Polly.Retry;

namespace AppHost.Extensions;

public static class DistributedApplicationExtensions
{
    public static IResourceBuilder<ParameterResource> CreateParameter(this IDistributedApplicationBuilder builder, string name, string value)
    {
        var resource = new ParameterResource(name, (_) => value);
        return builder.AddResource(resource);
    }

    public static async Task CreateDatabase(this DistributedApplication app, IResourceBuilder<PostgresDatabaseResource> db)
    {
        var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(DistributedApplicationExtensions));
        
        var dbName = db.Resource.DatabaseName;
        var connectionString = await db.Resource.Parent.GetConnectionStringAsync();

        logger.LogInformation("Opening db connection...");
        
        await using var connection = await CreateConnection(connectionString!);
        
        var command = new NpgsqlCommand($"CREATE DATABASE \"{dbName}\"", connection);
        await command.ExecuteNonQueryAsync();

        logger.LogInformation("Database {DbName} has been created", dbName);
    }

    private static async Task<NpgsqlConnection> CreateConnection(string connectionString)
    {
        var connection = new NpgsqlConnection(connectionString);

        var policy = Policy
            .Handle<NpgsqlException>()
            .WaitAndRetryAsync(
                retryCount: 5, 
                sleepDurationProvider: (retry) => TimeSpan.FromSeconds(2^retry)
            );
        
        await policy.ExecuteAsync(async () => await connection.OpenAsync());
        
        return connection;
    }
}