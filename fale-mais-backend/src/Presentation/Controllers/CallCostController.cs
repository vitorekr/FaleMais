using Microsoft.AspNetCore.Mvc;
using FaleMais.Application.DTOs;
using FaleMais.Application.Interfaces;
using System;

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
    public async Task<IActionResult> CalculateCallCost([FromBody] CalculateCallCostRequest request)
    {
        try
        {
            var result = await _callCostService.CalculateCallCostAsync(request.Origin, request.Destination, request.Duration, request.Plan);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ErrorResponse { Message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao calcular custo da chamada: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            return StatusCode(500, new ErrorResponse { Message = "Erro interno no servidor." });
        }
    }
}
