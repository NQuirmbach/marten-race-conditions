namespace EventConsumer.UserTasks.Events;

public record UserTaskUserAssigned(Guid StreamId, Guid UserId, string FirstName, string LastName);