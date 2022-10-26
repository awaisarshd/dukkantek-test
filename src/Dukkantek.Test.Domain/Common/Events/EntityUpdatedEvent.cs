namespace Dukkantek.Test.Domain.Common.Events;

public static class EntityUpdatedEvent
{
    public static EntityUpdatedEvent<TEntity> WithEntity<TEntity>(TEntity entity)
        where TEntity : IHasDomainEvent
        => new(entity);
}

public class EntityUpdatedEvent<TEntity> : DomainEvent
    where TEntity : IHasDomainEvent
{
    internal EntityUpdatedEvent(TEntity entity) => Entity = entity;

    public TEntity Entity { get; }
}