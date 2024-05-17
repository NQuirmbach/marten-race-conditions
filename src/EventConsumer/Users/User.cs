using EventConsumer.Persistence;
using ServiceDefaults.Messages;

namespace EventConsumer.Users;

public class User : Aggregate
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;

    public User(Guid id, string firstName, string lastName)
    {
        var created = new UserCreated(id, firstName, lastName);
        
        Apply(created);
        AddUncommittedEvent(created);
    }
    private User() {}

    [UsedImplicitly]
    public void Apply(UserCreated created)
    {
        Id = created.Id;
        FirstName = created.FirstName;
        LastName = created.LastName;

        Version++;
    }
}