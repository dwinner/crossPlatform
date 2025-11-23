using Microsoft.EntityFrameworkCore;
using StockTake.Domain.Entities;

namespace StockTake.App.Common.Interfaces;

public interface IApplicationDbContext
{
   DbSet<Product> Products { get; }

   DbSet<Manufacturer> Manufacturers { get; }

   DbSet<StockCount> StockCounts { get; }

   Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}