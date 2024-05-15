using EventConsumer.Persistence;
using ServiceDefaults.Messages;

namespace EventConsumer.Users;

public class UserCreatedHandler
{
    public async Task HandleAsync(UserCreated message, IEventStore store, CancellationToken cancellationToken)
    {
        var user = new User(message);
        
        await store.SaveAsync(user, cancellationToken);
    }
}