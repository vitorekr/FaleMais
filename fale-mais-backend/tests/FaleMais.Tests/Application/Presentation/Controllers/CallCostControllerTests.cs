using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FaleMais.Presentation.Controllers;
using FaleMais.Application.Interfaces;
using FaleMais.Application.DTOs;
using System;
using System.Threading.Tasks;

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
        public async Task CalculateCallCost_ValidRequest_ReturnsOkResult()
        {
            var request = new CalculateCallCostRequest { Origin = "011", Destination = "016", Duration = 20, Plan = "FaleMais 30" };
            var expectedResponse = new CalculateCallCostResponse { CostWithPlan = 10.00m, CostWithoutPlan = 30.00m };

            _mockService.Setup(s => s.CalculateCallCostAsync(request.Origin, request.Destination, request.Duration, request.Plan))
                        .ReturnsAsync(expectedResponse);

            var result = await _controller.CalculateCallCost(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<CalculateCallCostResponse>(okResult.Value);
            Assert.Equal(10.00m, response.CostWithPlan);
            Assert.Equal(30.00m, response.CostWithoutPlan);
        }

        [Fact]
        public async Task CalculateCallCost_InvalidRequest_ReturnsBadRequest()
        {
            var request = new CalculateCallCostRequest { Origin = "000", Destination = "999", Duration = 10, Plan = "" };

            _mockService
                .Setup(s => s.CalculateCallCostAsync(request.Origin, request.Destination, request.Duration, request.Plan))
                .ThrowsAsync(new ArgumentException("DDD inválido"));

            var result = await _controller.CalculateCallCost(request);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ErrorResponse>(badRequestResult.Value);

            Assert.NotNull(response);
            Assert.Equal("DDD inválido", response.Message);
        }

        [Fact]
        public async Task CalculateCallCost_ServiceThrowsException_ReturnsInternalServerError()
        {
            var request = new CalculateCallCostRequest { Origin = "011", Destination = "016", Duration = 10, Plan = "FaleMais 30" };

            _mockService
                .Setup(s => s.CalculateCallCostAsync(request.Origin, request.Destination, request.Duration, request.Plan))
                .ThrowsAsync(new Exception("Erro inesperado"));

            var result = await _controller.CalculateCallCost(request);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            var response = Assert.IsType<ErrorResponse>(objectResult.Value);
            Assert.Equal("Erro interno no servidor.", response.Message);
        }
    }
}
