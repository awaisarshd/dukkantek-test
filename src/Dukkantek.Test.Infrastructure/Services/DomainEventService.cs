using Dukkantek.Test.Application.Common.Interfaces;
using Dukkantek.Test.Application.Common.Models;
using Dukkantek.Test.Domain.Common;

using MediatR;

namespace Dukkantek.Test.Infrastructure.Services;

public class DomainEventService : IDomainEventService
{
    private readonly IPublisher _mediator;

    public DomainEventService(IPublisher mediator)
    {
        _mediator = mediator;
    }

    public async Task Publish(DomainEvent domainEvent)
    {
        await _mediator.Publish(GetNotificationCorrespondingToDomainEvent(domainEvent));
    }

    private INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
    {
        return (INotification)Activator.CreateInstance(
            typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent)!;
    }
}
