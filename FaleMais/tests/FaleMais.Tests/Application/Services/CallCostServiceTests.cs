using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using FaleMais.Application.Services;
using FaleMais.Application.DTOs;
using System;

namespace FaleMais.Tests.Application.Services
{
    public class CallCostServiceTests
    {
        private readonly CallCostService _service;
        private readonly Mock<ILogger<CallCostService>> _mockLogger;

        public CallCostServiceTests()
        {
            _mockLogger = new Mock<ILogger<CallCostService>>();
            _service = new CallCostService(_mockLogger.Object);
        }

        [Fact]
        public void CalculateCallCost_WithoutPlan_ReturnsCorrectValue()
        {
            var result = _service.CalculateCallCost("011", "016", 20, "");
            Assert.Equal(38.00m, result.CostWithoutPlan);
        }

        [Fact]
        public void CalculateCallCost_WithPlan_WithinFreeMinutes_ReturnsZero()
        {
            var result = _service.CalculateCallCost("011", "016", 30, "FaleMais 30");
            Assert.Equal(0m, result.CostWithPlan);
        }

        [Fact]
        public void CalculateCallCost_WithPlan_ExceedingFreeMinutes_ReturnsCorrectValue()
        {
            var result = _service.CalculateCallCost("011", "016", 40, "FaleMais 30");
            Assert.Equal(20.90m, result.CostWithPlan);
        }

        [Fact]
        public void CalculateCallCost_InvalidDDD_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                _service.CalculateCallCost("000", "999", 10, ""));

            Assert.Contains("Não há comunicação entre os DDDs informados", exception.Message);
        }

        [Fact]
        public void CalculateCallCost_InvalidPlan_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                _service.CalculateCallCost("011", "016", 10, "Plano Inexistente"));

            Assert.Contains("O plano 'Plano Inexistente' não existe", exception.Message);
        }

        [Fact]
        public void CalculateCallCost_InvalidDuration_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                _service.CalculateCallCost("011", "016", 0, "FaleMais 30"));

            Assert.Contains("A duração da chamada deve ser maior que zero", exception.Message);
        }

        [Fact]
        public void CalculateCallCost_EmptyOrigin_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                _service.CalculateCallCost("", "016", 10, "FaleMais 30"));

            Assert.Contains("Origem e destino não podem estar vazios", exception.Message);
        }

        [Fact]
        public void CalculateCallCost_EmptyDestination_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                _service.CalculateCallCost("011", "", 10, "FaleMais 30"));

            Assert.Contains("Origem e destino não podem estar vazios", exception.Message);
        }
    }
}
