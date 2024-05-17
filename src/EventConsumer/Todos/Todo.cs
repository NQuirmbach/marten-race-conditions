using EventConsumer.Persistence;
using EventConsumer.Todos.Events;
using EventConsumer.Users;
using ServiceDefaults.Messages;

namespace EventConsumer.Todos;

public class Todo : Aggregate
{
    public string Description { get; private set; } = null!;
    public User? CreatedBy { get; private set; }
    public User? ChangedBy { get; private set; }
    
    
    public Todo(Guid id, string description)
    {
        var create = new CreateTodo(id, description);
        
        Apply(create);
        AddUncommittedEvent(create);
    }

    private Todo()
    { }
    
    [UsedImplicitly]
    public void Apply(CreateTodo created)
    {
        Id = created.Id;
        Description = created.Description;

        Version++;
    }

    public void AssignUser(Guid userId, UserAssignment assignment)
    {
        AddUncommittedEvent(new TodoUserAssigned(Id, userId, assignment));
        Version++;
    }

    public void SetAssignment(User user, UserAssignment assignment)
    {
        switch (assignment)
        {
            case UserAssignment.Created:
                CreatedBy = user;
                break;
            case UserAssignment.Changed:
                ChangedBy = user;
                break;
            default:
                throw new NotImplementedException();
        }
    }
}

public enum UserAssignment
{
    Created,
    Changed
}