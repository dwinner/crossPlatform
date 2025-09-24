using MauiStockTake.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MauiStockTake.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }

    DbSet<Manufacturer> Manufacturers { get; }

    DbSet<StockCount> StockCounts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
