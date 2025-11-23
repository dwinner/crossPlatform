using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockTake.App.Inventory.Commands;
using StockTake.App.Inventory.Queries;
using StockTake.Shared.Inventory;
using StockTake.Shared.StockCounts;

namespace StockTake.WebServices.Controllers;

[Authorize]
public class InventoryController : ApiControllerBase
{
   [HttpPost]
   [ProducesResponseType(StatusCodes.Status200OK)]
   public async Task<ActionResult> AddStockCount(StockCountDto stockCount)
   {
      var sendTask = Mediator.Send(new AddStockCountCommand { Count = stockCount });
      var result = new object();
      return Ok(result);
   }

   [HttpGet]
   public async Task<ActionResult<InventoryVm>> GetInventory() => Ok(await Mediator.Send(new GetInventoryQuery()));
}