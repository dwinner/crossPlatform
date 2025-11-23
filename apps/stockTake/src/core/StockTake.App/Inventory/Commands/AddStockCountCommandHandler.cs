using MediatR;
using StockTake.App.Common.Interfaces;
using StockTake.Domain.Entities;

namespace StockTake.App.Inventory.Commands;

public class AddStockCountCommandHandler : IRequestHandler<AddStockCountCommand>
{
   private readonly IApplicationDbContext _context;
   private readonly ICurrentUserService _currentUserService;

   public AddStockCountCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
   {
      _context = context;
      _currentUserService = currentUserService;
   }

   public async Task Handle(AddStockCountCommand request, CancellationToken cancellationToken)
   {
      var stockCount = new StockCount
      {
         ProductId = request.Count.ProductId,
         CountedAt = DateTime.UtcNow,
         Count = request.Count.ProductCount,
         CountedById = _currentUserService.UserId
      };

      _context.StockCounts.Add(stockCount);

      await _context.SaveChangesAsync(cancellationToken);
   }
}