namespace EventConsumer.Todos.Events;

public record CreateTodo(Guid Id, string Description);