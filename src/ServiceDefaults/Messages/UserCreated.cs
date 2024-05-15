using Wolverine;

namespace ServiceDefaults.Messages;

public class UserCreated : IMessage
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
}