using EventConsumer.Todos;
using EventConsumer.Users;
using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using ServiceDefaults;
using Weasel.Core;

namespace EventConsumer.Persistence;

public static class Config
{
    public static void AddMartenDefaults(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMarten(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString(Const.ConnectionStrings.Postgres)!;
            options.Connection(connectionString);

            if (builder.Environment.IsDevelopment())
            {
                options.AutoCreateSchemaObjects = AutoCreate.All;
            }
            
            AddProjections(options.Projections);
        })
        // Turn on the async daemon in "Solo" mode
        .AddAsyncDaemon(DaemonMode.Solo);
        
        builder.Services.AddScoped<IEventRepository, MartenEventRepository>();
    }

    private static void AddProjections(ProjectionOptions p)
    {
        p.Add<UserProjection>(ProjectionLifecycle.Async);
        p.Add<TodoProjection>(ProjectionLifecycle.Async);
    }
}