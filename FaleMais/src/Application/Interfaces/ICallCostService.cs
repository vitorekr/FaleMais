using FaleMais.Application.DTOs;

namespace FaleMais.Application.Interfaces;

public interface ICallCostService
{
    CalculateCallCostResponse CalculateCallCost(string origin, string destination, int duration, string plan);
}
