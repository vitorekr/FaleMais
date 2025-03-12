using FaleMais.Application.DTOs;
using FaleMais.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace FaleMais.Application.Services;

public class CallCostService : ICallCostService
{
    private readonly ILogger<CallCostService> _logger;

    private static readonly Dictionary<(string, string), decimal> _callRates = new()
    {
        { ("011", "016"), 1.90m }, { ("016", "011"), 2.90m },
        { ("011", "017"), 1.70m }, { ("017", "011"), 2.70m },
        { ("011", "018"), 0.90m }, { ("018", "011"), 1.90m }
    };

    private static readonly Dictionary<string, int> _plans = new()
    {
        { "FaleMais 30", 30 }, { "FaleMais 60", 60 }, { "FaleMais 120", 120 }
    };

    public CallCostService(ILogger<CallCostService> logger)
    {
        _logger = logger;
    }

    public CalculateCallCostResponse CalculateCallCost(string origin, string destination, int duration, string plan)
    {
        try
        {
            ValidateInputs(origin, destination, duration, plan);

            decimal rate = _callRates[(origin, destination)];
            int freeMinutes = _plans.GetValueOrDefault(plan, 0);

            decimal costWithoutPlan = duration * rate;
            decimal costWithPlan = Math.Max(0, duration - freeMinutes) * rate * 1.10m;

            _logger.LogInformation($"Cálculo realizado: Com plano: R$ {costWithPlan}, Sem plano: R$ {costWithoutPlan}");

            return new CalculateCallCostResponse { CostWithPlan = costWithPlan, CostWithoutPlan = costWithoutPlan };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao calcular custo: {ex.Message}");
            throw;
        }
    }

    private void ValidateInputs(string origin, string destination, int duration, string plan)
    {
        if (string.IsNullOrWhiteSpace(origin) || string.IsNullOrWhiteSpace(destination))
            throw new ArgumentException("Origem e destino não podem estar vazios.");

        if (duration <= 0)
            throw new ArgumentException("A duração da chamada deve ser maior que zero.");

        if (!_callRates.ContainsKey((origin, destination)))
            throw new ArgumentException($"Não há comunicação entre os DDDs informados ({origin} -> {destination}).");

        if (!string.IsNullOrWhiteSpace(plan) && !_plans.ContainsKey(plan))
            throw new ArgumentException($"O plano '{plan}' não existe. Escolha um dos planos disponíveis.");
    }
}
