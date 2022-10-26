namespace Dukkantek.Test.Domain.Common.Events;

public static class EntityCreatedEvent
{
    public static EntityCreatedEvent<TEntity> WithEntity<TEntity>(TEntity entity)
        where TEntity : IHasDomainEvent
        => new(entity);
}

public class EntityCreatedEvent<TEntity> : DomainEvent
    where TEntity : IHasDomainEvent
{
    internal EntityCreatedEvent(TEntity entity) => Entity = entity;

    public TEntity Entity { get; }
}