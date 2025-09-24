using MauiStockTake.Application.Inventory.Commands;
using MauiStockTake.Application.Inventory.Queries;
using MauiStockTake.Shared.Inventory.Queries;
using MauiStockTake.Shared.StockCounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MauiStockTake.WebUI.Controllers;

[Authorize]
public class InventoryController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> AddStockCount(StockCountDto stockCount)
    {
        return Ok(await Mediator.Send(new AddStockCountCommand { Count = stockCount}));
    }

    [HttpGet]
    public async Task<ActionResult<InventoryVm>> GetInventory()
    {
        return Ok(await Mediator.Send(new GetInventoryQuery()));
    }
}
