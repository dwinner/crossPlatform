using MauiStockTake.Application.Common.Interfaces;

namespace MauiStockTake.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
