using Microsoft.AspNetCore.Mvc;
using UnoBookRail.Common.Network;

namespace UnoBookRail.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StationsController : ControllerBase
{
    [HttpGet]
    public Arrivals GetNextArrivals(int stationId) => new Stations().GetNextArrivals(stationId);
}