using MauiStockTake.Application.Common.Interfaces;
using MauiStockTake.Domain.Entities;
using MauiStockTake.Shared.StockCounts;
using MediatR;

namespace MauiStockTake.Application.Inventory.Commands;
public class AddStockCountCommand : IRequest
{
    public StockCountDto Count { get; set; }
}

public class AddStockCountCommandHandler : IRequestHandler<AddStockCountCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AddStockCountCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(AddStockCountCommand request, CancellationToken cancellationToken)
    {
        var stockCount = new StockCount
        {
            ProductId = request.Count.ProductId,
            CountedAt = DateTime.UtcNow,
            Count = request.Count.ProductCount,
            CountedById = _currentUserService.UserId,
        };

        _context.StockCounts.Add(stockCount);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
