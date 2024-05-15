using AppHost.Extensions;
using Microsoft.Extensions.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgresUsername = builder.CreateParameter("postgres-user", "user");
var postgresPassword = builder.CreateParameter("postgres-password", "password");

var postgres = builder.AddPostgres("postgres", userName: postgresUsername, password: postgresPassword, port: 5432);

var rabbitUsername = builder.CreateParameter("rabbit-user", "user");
var rabbitPassword = builder.CreateParameter("rabbit-password", "password");

var rabbit = builder.AddRabbitMQ("rabbitmq", userName: rabbitUsername, password: rabbitPassword, port: 5672)
    .WithManagementPlugin();

builder.AddProject<EventProducer>("event-producer")
    .WithReference(rabbit, "RabbitMQ");

var consumerDb = postgres.AddDatabase("consumer");
builder.AddProject<EventConsumer>("event-consumer")
    .WithReference(consumerDb, "Postgres")
    .WithReference(rabbit, "RabbitMQ");

var app = builder.Build();

await app.StartAsync();

await app.CreateDatabase(consumerDb);

await app.WaitForShutdownAsync();
