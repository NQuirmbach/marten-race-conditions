using Marten.Events.Aggregation;
using Marten.Events.Projections;
using Marten.Internal.Sessions;
using ServiceDefaults.Messages;

namespace EventConsumer.UserTasks;

public class UserTaskProjection : EventProjection
{
    public UserTaskProjection()
    {
        
    }
}
