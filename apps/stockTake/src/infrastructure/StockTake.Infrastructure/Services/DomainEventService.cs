using MediatR;
using Microsoft.Extensions.Logging;
using StockTake.App.Common.Interfaces;
using StockTake.App.Common.Models;
using StockTake.Domain.Common;

namespace StockTake.Infrastructure.Services;

public class DomainEventService : IDomainEventService
{
   private readonly ILogger<DomainEventService> _logger;
   private readonly IPublisher _mediator;

   public DomainEventService(ILogger<DomainEventService> logger, IPublisher mediator)
   {
      _logger = logger;
      _mediator = mediator;
   }

   public async Task Publish(DomainEvent domainEvent)
   {
      _logger.LogInformation("Publishing domain event. Event - {event}", domainEvent.GetType().Name);
      await _mediator.Publish(GetNotificationCorrespondingToDomainEvent(domainEvent));
   }

   private INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent) =>
      (INotification)Activator.CreateInstance(
         typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent)!;
}