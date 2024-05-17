using EventConsumer.Persistence;
using EventConsumer.Users;
using JetBrains.Annotations;
using ServiceDefaults.Messages;

namespace EventConsumer.Todos;

public class Todo : Aggregate
{
    public string Description { get; private set; } = null!;
    public TodoUser CreatedBy { get; private set; }
    public TodoUser ChangedBy { get; private set; }
    
    
    public Todo(TodoCreated created)
    {
        Apply(created);
        AddUncommittedEvent(created);
    }
    
    private Todo() { }

    [UsedImplicitly]
    public void Apply(TodoCreated created)
    {
        Id = created.Id;
        Description = created.Description;
        CreatedBy = new TodoUser(created.CreatedBy);
        ChangedBy = new TodoUser(ChangedBy.Id);

        Version++;
    }

    public void SetCreatedBy(string firstName, string lastName)
    {
        CreatedBy = CreatedBy with
        {
            FirstName = firstName,
            Lastname = lastName
        };
    }

    public void SetChangedBy(string firstName, string lastName)
    {
        ChangedBy = ChangedBy with
        {
            FirstName = firstName,
            Lastname = lastName
        };
    }
}