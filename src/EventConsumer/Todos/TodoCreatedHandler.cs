using EventConsumer.Persistence;
using ServiceDefaults.Messages;

namespace EventConsumer.Todos;

public class TodoCreatedHandler
{
    public async Task HandleAsync(TodoCreated message, IEventStore store, CancellationToken cancellationToken)
    {
        var task = new Todo(message);

        await store.SaveAsync(task, cancellationToken);
    }
}