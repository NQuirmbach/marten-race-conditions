using EventConsumer.Todos.Events;
using Marten.Events.Projections;

namespace EventConsumer.Todos;

public class TodoProjection : MultiStreamProjection<Todo, Guid>
{
    public TodoProjection()
    {
        Identity<CreateTodo>(e => e.Id);
        Identity<TodoUserAssigned>(e => e.StreamId);
    }
    
    [UsedImplicitly]
    public Todo Create(CreateTodo e) => new(e.Id, e.Description);

    // [UsedImplicitly]
    // public async Task Project(TodoUserAssigned e, IDocumentOperations ops)
    // {
    //     var todo = await ops.LoadAsync<Todo>(e.StreamId);
    //     var user = await ops.LoadAsync<User>(e.UserId);
    //
    //     if (todo is null || user is null) return;
    //     
    //     todo.SetAssignment(user, e.Assignment);
    // }
}
