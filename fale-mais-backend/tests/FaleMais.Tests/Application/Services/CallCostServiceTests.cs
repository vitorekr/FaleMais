using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using FaleMais.Application.Services;
using FaleMais.Application.DTOs;
using FaleMais.Domain.Repositories;
using FaleMais.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace FaleMais.Tests.Application.Services
{
    public class CallCostServiceTests
    {
        private readonly CallCostService _service;
        private readonly Mock<ILogger<CallCostService>> _mockLogger;
        private readonly Mock<ITarifaRepository> _mockTarifaRepository;
        private readonly Mock<IPlanoRepository> _mockPlanoRepository;

        public CallCostServiceTests()
        {
            _mockLogger = new Mock<ILogger<CallCostService>>();
            _mockTarifaRepository = new Mock<ITarifaRepository>();
            _mockPlanoRepository = new Mock<IPlanoRepository>();

            _service = new CallCostService(_mockTarifaRepository.Object, _mockPlanoRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task CalculateCallCost_WithoutPlan_ReturnsCorrectValue()
        {
            _mockTarifaRepository
                .Setup(repo => repo.GetTarifaAsync("011", "016"))
                .ReturnsAsync(new Tarifa { Origem = "011", Destino = "016", Valor = 1.90m });

            var result = await _service.CalculateCallCostAsync("011", "016", 20, "");

            Assert.Equal(38.00m, result.CostWithoutPlan);
        }

        [Fact]
        public async Task CalculateCallCost_WithPlan_WithinFreeMinutes_ReturnsZero()
        {
            _mockTarifaRepository
                .Setup(repo => repo.GetTarifaAsync("011", "016"))
                .ReturnsAsync(new Tarifa { Origem = "011", Destino = "016", Valor = 1.90m });

            _mockPlanoRepository
                .Setup(repo => repo.GetPlanoAsync("FaleMais 30"))
                .ReturnsAsync(new Plano { Nome = "FaleMais 30", MinutosGratis = 30 });

            var result = await _service.CalculateCallCostAsync("011", "016", 30, "FaleMais 30");

            Assert.Equal(0m, result.CostWithPlan);
        }

        [Fact]
        public async Task CalculateCallCost_WithPlan_ExceedingFreeMinutes_ReturnsCorrectValue()
        {
            _mockTarifaRepository
                .Setup(repo => repo.GetTarifaAsync("011", "016"))
                .ReturnsAsync(new Tarifa { Origem = "011", Destino = "016", Valor = 1.90m });

            _mockPlanoRepository
                .Setup(repo => repo.GetPlanoAsync("FaleMais 30"))
                .ReturnsAsync(new Plano { Nome = "FaleMais 30", MinutosGratis = 30 });

            var result = await _service.CalculateCallCostAsync("011", "016", 40, "FaleMais 30");

            Assert.Equal(20.90m, result.CostWithPlan);
        }

        [Fact]
        public async Task CalculateCallCost_InvalidDDD_ThrowsArgumentException()
        {
            _mockTarifaRepository
                .Setup(repo => repo.GetTarifaAsync("000", "999"))
                .ReturnsAsync((Tarifa?)null);

            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.CalculateCallCostAsync("000", "999", 10, ""));

            Assert.Contains("Não há comunicação entre os DDDs", exception.Message);
        }

        [Fact]
        public async Task CalculateCallCost_InvalidPlan_ThrowsArgumentException()
        {
            _mockTarifaRepository
                .Setup(repo => repo.GetTarifaAsync("011", "016"))
                .ReturnsAsync(new Tarifa { Origem = "011", Destino = "016", Valor = 1.90m });

            _mockPlanoRepository
                .Setup(repo => repo.GetPlanoAsync("Plano Inexistente"))
                .ReturnsAsync((Plano?)null);

            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.CalculateCallCostAsync("011", "016", 10, "Plano Inexistente"));

            Assert.Contains("O plano 'Plano Inexistente' não existe", exception.Message);
        }

        [Fact]
        public async Task CalculateCallCost_InvalidDuration_ThrowsArgumentException()
        {
            _mockTarifaRepository
                .Setup(repo => repo.GetTarifaAsync("011", "016"))
                .ReturnsAsync(new Tarifa { Origem = "011", Destino = "016", Valor = 1.90m });

            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.CalculateCallCostAsync("011", "016", 0, "FaleMais 30"));

            Assert.Contains("A duração da chamada deve ser maior que zero", exception.Message);
        }

        [Fact]
        public async Task CalculateCallCost_EmptyOrigin_ThrowsArgumentException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.CalculateCallCostAsync("", "016", 10, "FaleMais 30"));

            Assert.Contains("Origem e destino não podem estar vazios", exception.Message);
        }

        [Fact]
        public async Task CalculateCallCost_EmptyDestination_ThrowsArgumentException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.CalculateCallCostAsync("011", "", 10, "FaleMais 30"));

            Assert.Contains("Origem e destino não podem estar vazios", exception.Message);
        }
    }
}
