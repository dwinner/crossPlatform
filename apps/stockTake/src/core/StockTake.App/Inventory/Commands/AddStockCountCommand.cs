using MediatR;
using StockTake.Shared.StockCounts;

namespace StockTake.App.Inventory.Commands;

public class AddStockCountCommand : IRequest
{
   public StockCountDto Count { get; set; }
}