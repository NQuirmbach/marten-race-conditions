using Wolverine;

namespace ServiceDefaults.Messages;

[UsedImplicitly]
public class TodoCreated : IMessage
{
    public Guid Id { get; init; }
    public string Description { get; init; } = null!;
    public Guid CreatedBy { get; init; }
    public Guid ChangedBy { get; set; }
}