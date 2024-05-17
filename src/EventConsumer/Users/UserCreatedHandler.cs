using EventConsumer.Persistence;
using ServiceDefaults.Messages;

namespace EventConsumer.Users;

public class UserCreatedHandler
{
    public async Task HandleAsync(UserCreated message, IEventRepository repository, CancellationToken cancellationToken)
    {
        var user = new User(message.Id, message.FirstName, message.LastName);
        
        await repository.SaveAsync(user, cancellationToken);
    }
}