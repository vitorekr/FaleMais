using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using FaleMais.Application.Services;
using FaleMais.Application.DTOs;

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
        public void CalculateCallCost_WithPlan_ReturnsDiscountedValue()
        {
            var result = _service.CalculateCallCost("011", "016", 40, "FaleMais 30");
            Assert.Equal(20.90m, result.CostWithPlan);
        }

        [Fact]
        public void CalculateCallCost_InvalidDDD_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                _service.CalculateCallCost("000", "999", 10, ""));
        }
    }
}
