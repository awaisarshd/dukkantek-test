namespace Dukkantek.Test.Domain.Common.Events;

public static class EntityDeletedEvent
{
    public static EntityDeletedEvent<TEntity> WithEntity<TEntity>(TEntity entity)
        where TEntity : IHasDomainEvent
        => new(entity);
}

public class EntityDeletedEvent<TEntity> : DomainEvent
    where TEntity : IHasDomainEvent
{
    internal EntityDeletedEvent(TEntity entity) => Entity = entity;

    public TEntity Entity { get; }
}
