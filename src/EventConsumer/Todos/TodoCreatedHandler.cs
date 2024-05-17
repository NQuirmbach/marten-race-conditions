using EventConsumer.Persistence;
using ServiceDefaults.Messages;

namespace EventConsumer.Todos;

public class TodoCreatedHandler
{
    public async Task HandleAsync(TodoCreated message, IEventRepository repository, CancellationToken cancellationToken)
    {
        var todo = new Todo(message.Id, message.Description);
        
        todo.AssignUser(message.CreatedBy, UserAssignment.Created);
        todo.AssignUser(message.ChangedBy, UserAssignment.Changed);
        
        await repository.SaveAsync(todo, cancellationToken);
    }
}