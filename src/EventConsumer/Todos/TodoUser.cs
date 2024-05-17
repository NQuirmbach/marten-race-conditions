namespace EventConsumer.Todos;

public record TodoUser(Guid Id, string? FirstName = null, string? Lastname = null);