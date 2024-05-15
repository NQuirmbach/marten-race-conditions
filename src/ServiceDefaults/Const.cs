namespace ServiceDefaults;

public class Const
{
    public class ConnectionStrings
    {
        public const string RabbitMQ = nameof(RabbitMQ);
        public const string Postgres = nameof(Postgres);
    }
    
    public class Queues
    {
        public const string UserCreated = "user-created";
        public const string TaskCreated = "task-created";
    }
}