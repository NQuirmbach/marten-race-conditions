using Bogus;
using ServiceDefaults.Messages;
using Wolverine;

namespace EventProducer.Messages;

public record GenerateEvents();

public class GenerateEventsHandler
{
    private readonly ILogger<GenerateEventsHandler> _logger;
    private readonly Random _random;

    public GenerateEventsHandler(ILogger<GenerateEventsHandler> logger)
    {
        _logger = logger;
        _random = new Random();
    }
    
    public IEnumerable<IMessage> Handle(GenerateEvents message)
    {
        _logger.LogInformation("Generating events");

        var users = GenerateUserEvents();
        
        _logger.LogInformation("Generated {Count} user events", users.Length);
        var tasks = GenerateTaskEvents(users);

        _logger.LogInformation("Generated {Count} task events", tasks.Length);

        var result = new List<IMessage>();
        
        result.AddRange(users);
        result.AddRange(tasks);

        return result;
    }

    private static UserCreated[] GenerateUserEvents()
    {
        var faker = new UserCreatedFaker();

        return faker.Generate(50).ToArray();
    }
    
    private static TaskCreated[] GenerateTaskEvents(IEnumerable<UserCreated> events)
    {
        var userIds = events.Select(m => m.Id).Distinct().ToArray();
        var faker = new TaskCreatedFaker(userIds);

        return faker.Generate(100).ToArray();
    }
}

public sealed class UserCreatedFaker : Faker<UserCreated>
{
    public UserCreatedFaker()
    {
        RuleFor(m => m.Id, _ => Guid.NewGuid());
        RuleFor(m => m.FirstName, f => f.Name.FirstName());
        RuleFor(m => m.LastName, f => f.Name.LastName());
    }
}

public sealed class TaskCreatedFaker : Faker<TaskCreated>
{
    public TaskCreatedFaker(Guid[] userIds)
    {
        RuleFor(m => m.Id, _ => Guid.NewGuid());
        RuleFor(m => m.Description, f => f.Commerce.Product());
        RuleFor(m => m.UserId, f => f.PickRandom(userIds));
    }
}