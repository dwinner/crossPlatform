using StockTake.Domain.Common;

namespace StockTake.App.Common.Interfaces;

public interface IDomainEventService
{
   Task Publish(DomainEvent domainEvent);
}