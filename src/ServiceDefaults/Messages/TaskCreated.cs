using Wolverine;

namespace ServiceDefaults.Messages;

[UsedImplicitly]
public class TaskCreated : IMessage
{
    public Guid Id { get; init; }
    public string Description { get; init; } = null!;
    public Guid UserId { get; init; }
}