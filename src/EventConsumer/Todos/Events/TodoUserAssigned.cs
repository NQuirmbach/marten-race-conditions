namespace EventConsumer.Todos.Events;

public record TodoUserAssigned(Guid StreamId, Guid UserId, string FirstName, string LastName);