using MediatR;
using StockTake.Shared.Inventory;

namespace StockTake.App.Inventory.Queries;

public class GetInventoryQuery : IRequest<InventoryVm>
{
}