using Microsoft.AspNetCore.Mvc;
using FaleMais.Application.DTOs;
using FaleMais.Application.Interfaces;

namespace FaleMais.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CallCostController : ControllerBase
{
    private readonly ICallCostService _callCostService;

    public CallCostController(ICallCostService callCostService)
    {
        _callCostService = callCostService;
    }

    [HttpPost("calculate")]
    public IActionResult CalculateCallCost([FromBody] CalculateCallCostRequest request)
    {
        var result = _callCostService.CalculateCallCost(request.Origin, request.Destination, request.Duration, request.Plan);

        if (result.CostWithoutPlan == -1)
        {
            return BadRequest("Invalid origin or destination.");
        }

        return Ok(result);
    }
}