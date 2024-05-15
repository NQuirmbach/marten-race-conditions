using EventConsumer.Persistence;
using JetBrains.Annotations;
using ServiceDefaults.Messages;

namespace EventConsumer.Users;

public class User : Aggregate
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;

    public User(UserCreated created)
    {
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