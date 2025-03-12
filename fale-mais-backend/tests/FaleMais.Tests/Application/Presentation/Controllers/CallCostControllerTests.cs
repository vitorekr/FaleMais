using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FaleMais.Presentation.Controllers;
using FaleMais.Application.Interfaces;
using FaleMais.Application.DTOs;

namespace FaleMais.Tests.Presentation.Controllers
{
    public class CallCostControllerTests
    {
        private readonly Mock<ICallCostService> _mockService;
        private readonly CallCostController _controller;

        public CallCostControllerTests()
        {
            _mockService = new Mock<ICallCostService>();
            _controller = new CallCostController(_mockService.Object);
        }

        [Fact]
        public void CalculateCallCost_ValidRequest_ReturnsOkResult()
        {
            var request = new CalculateCallCostRequest { Origin = "011", Destination = "016", Duration = 20, Plan = "FaleMais 30" };
            var expectedResponse = new CalculateCallCostResponse { CostWithPlan = 10.00m, CostWithoutPlan = 30.00m };

            _mockService.Setup(s => s.CalculateCallCost(request.Origin, request.Destination, request.Duration, request.Plan))
                        .Returns(expectedResponse);

            var result = _controller.CalculateCallCost(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<CalculateCallCostResponse>(okResult.Value);
            Assert.Equal(10.00m, response.CostWithPlan);
            Assert.Equal(30.00m, response.CostWithoutPlan);
        }

        [Fact]
        public void CalculateCallCost_InvalidRequest_ReturnsBadRequest()
        {
            var request = new CalculateCallCostRequest { Origin = "000", Destination = "999", Duration = 10, Plan = "" };

            _mockService
                .Setup(s => s.CalculateCallCost(request.Origin, request.Destination, request.Duration, request.Plan))
                .Throws(new ArgumentException("DDD inválido"));

            var result = _controller.CalculateCallCost(request);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            var response = Assert.IsType<ErrorResponse>(badRequestResult.Value);

            Assert.NotNull(response);
            Assert.Equal("DDD inválido", response.Message);
        }

    }
}
