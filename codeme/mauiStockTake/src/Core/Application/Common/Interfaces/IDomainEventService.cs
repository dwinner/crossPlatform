using MauiStockTake.Domain.Common;

namespace MauiStockTake.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}
