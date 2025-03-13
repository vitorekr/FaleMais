using FaleMais.Application.DTOs;
using System.Threading.Tasks;

namespace FaleMais.Application.Interfaces
{
    public interface ICallCostService
    {
        Task<CalculateCallCostResponse> CalculateCallCostAsync(string origin, string destination, int duration, string plan);
    }
}

