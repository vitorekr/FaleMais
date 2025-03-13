using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using FaleMais.Application.Interfaces;
using FaleMais.Domain.Repositories;
using FaleMais.Application.DTOs;

namespace FaleMais.Application.Services
{
    public class CallCostService : ICallCostService
    {
        private readonly ITarifaRepository _tarifaRepository;
        private readonly IPlanoRepository _planoRepository;
        private readonly ILogger<CallCostService> _logger;

        public CallCostService(
            ITarifaRepository tarifaRepository,
            IPlanoRepository planoRepository,
            ILogger<CallCostService> logger)
        {
            _tarifaRepository = tarifaRepository;
            _planoRepository = planoRepository;
            _logger = logger;
        }

        public async Task<CalculateCallCostResponse> CalculateCallCostAsync(string origin, string destination, int duration, string plan)
        {
            _logger.LogInformation($"Calculando custo de chamada: Origem={origin}, Destino={destination}, Duração={duration}, Plano={plan}");

            if (string.IsNullOrWhiteSpace(origin) || string.IsNullOrWhiteSpace(destination))
            {
                throw new ArgumentException("Origem e destino não podem estar vazios");
            }

            if (duration <= 0)
            {
                throw new ArgumentException("A duração da chamada deve ser maior que zero");
            }

            var tarifa = await _tarifaRepository.GetTarifaAsync(origin, destination);
            if (tarifa == null)
                throw new ArgumentException("Não há comunicação entre os DDDs");

            var plano = await _planoRepository.GetPlanoAsync(plan);
            if (!string.IsNullOrEmpty(plan) && plano == null)
            {
                throw new ArgumentException($"O plano '{plan}' não existe");
            }

            int minutosGratis = plano?.MinutosGratis ?? 0;
            decimal custoSemPlano = duration * tarifa.Valor;
            decimal custoComPlano = Math.Max(0, duration - minutosGratis) * tarifa.Valor * 1.10m;

            return new CalculateCallCostResponse { CostWithPlan = custoComPlano, CostWithoutPlan = custoSemPlano };
        }
    }
}
