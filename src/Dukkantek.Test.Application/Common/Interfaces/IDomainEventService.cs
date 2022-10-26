using Dukkantek.Test.Domain.Common;

namespace Dukkantek.Test.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}
