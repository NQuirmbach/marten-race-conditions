using Marten;

namespace EventConsumer.Persistence;

public interface IEventRepository
{
    Task SaveAsync<T>(T aggregate, CancellationToken cancellationToken = default) where T : Aggregate;
    IDocumentSession OpenSession();
}

public class MartenEventRepository : IEventRepository
{
    private readonly IDocumentStore _store;

    public MartenEventRepository(IDocumentStore store)
    {
        _store = store;
    }
    
    public async Task SaveAsync<T>(T aggregate, CancellationToken cancellationToken = default)
        where T : Aggregate
    {
        await using var session = _store.LightweightSession();
        
        // Take non-persisted events, push them to the event stream, indexed by the aggregate ID
        var events = aggregate.GetUncommittedEvents().ToArray();
        
        session.Events.Append(aggregate.Id, aggregate.Version, events);
        await session.SaveChangesAsync(cancellationToken);
        
        // Once successfully persisted, clear events from list of uncommitted events
        aggregate.ClearUncommittedEvents();
    }

    public IDocumentSession OpenSession() => _store.LightweightSession();
}