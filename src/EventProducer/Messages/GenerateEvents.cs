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
        var todos = GenerateTodoEvents(users);

        _logger.LogInformation("Generated {Count} todo events", todos.Length);

        var result = new List<IMessage>();
        
        result.AddRange(users);
        result.AddRange(todos);

        return result;
    }

    private static UserCreated[] GenerateUserEvents()
    {
        var faker = new UserCreatedFaker();

        return faker.Generate(50).ToArray();
    }
    
    private static TodoCreated[] GenerateTodoEvents(IEnumerable<UserCreated> events)
    {
        var userIds = events.Select(m => m.Id).Distinct().ToArray();
        var faker = new TodoCreatedFaker(userIds);

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

public sealed class TodoCreatedFaker : Faker<TodoCreated>
{
    public TodoCreatedFaker(Guid[] userIds)
    {
        RuleFor(m => m.Id, _ => Guid.NewGuid());
        RuleFor(m => m.Description, f => f.Commerce.Product());
        RuleFor(m => m.CreatedBy, f => f.PickRandom(userIds));
        RuleFor(m => m.ChangedBy, f => f.PickRandom(userIds));
    }
}