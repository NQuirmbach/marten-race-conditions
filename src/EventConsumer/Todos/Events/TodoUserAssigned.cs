namespace EventConsumer.Todos.Events;

public record TodoUserAssigned(Guid StreamId, Guid UserId, UserAssignment Assignment);