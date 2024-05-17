using EventConsumer.Persistence;
using ServiceDefaults.Messages;

namespace EventConsumer.UserTasks;

public class TaskCreatedHandler
{
    public async Task HandleAsync(TaskCreated message, IEventStore store, CancellationToken cancellationToken)
    {
        var task = new UserTask(message);

        await store.SaveAsync(task, cancellationToken);
    }
}