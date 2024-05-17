using Wolverine;

namespace ServiceDefaults.Messages;

[UsedImplicitly]
public class UserCreated : IMessage
{
    public UserCreated(Guid id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        
    }
    public UserCreated()
    { }
    
    public Guid Id { get; init; }
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
}