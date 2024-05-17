using EventConsumer.Persistence;
using EventConsumer.UserTasks.Events;
using JetBrains.Annotations;
using ServiceDefaults.Messages;

namespace EventConsumer.UserTasks;

public class UserTask : Aggregate
{
    public string Description { get; private set; } = null!;
    public Guid UserId { get; private set; }
    public Username? Username { get; private set; }
    
    
    public UserTask(TaskCreated created)
    {
        Apply(created);
        AddUncommittedEvent(created);
    }
    
    private UserTask() { }

    [UsedImplicitly]
    public void Apply(TaskCreated created)
    {
        Id = created.Id;
        Description = created.Description;
        UserId = created.UserId;

        Version++;
    }

    [UsedImplicitly]
    public void Apply(UserTaskUserAssigned e)
    {
        if (e.UserId != UserId)
            throw new ArgumentException("Invalid user id");
        
        Username = new Username(e.FirstName, e.LastName);
        Version++;
    }
}