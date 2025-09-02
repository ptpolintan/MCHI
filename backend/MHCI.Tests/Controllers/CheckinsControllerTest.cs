using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MHCI.Api.Controllers;
using MHCI.Application.Interfaces;
using MHCI.Application.Models;
using MHCI.Api.DTOs.Responses;

namespace MHCI.Tests.Controllers
{
    [TestFixture]
    public class CheckInsControllerTests
    {
        private Mock<ICheckInService> _mockService;
        private CheckInsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<ICheckInService>();

            _controller = new CheckInsController(_mockService.Object);

            // Mock HttpContext with current user id
            var httpContext = new DefaultHttpContext();
            httpContext.Items["currentuserid"] = 1;
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
            _controller = null!;
            _mockService = null!;
        }

        [Test]
        public async Task GetCheckins_ReturnsOk_WithExpectedData()
        {
            // Arrange
            var expectedResult = new Application.Models.Responses.GetCheckInsResponseDTO
            {
                TotalRecords = 2,
                Data = new List<CheckInModel>
                {
                    new CheckInModel { Id = 1, Mood = 3, Notes = "Good", UserId = 1 },
                    new CheckInModel { Id = 2, Mood = 4, Notes = "Great", UserId = 1 }
                },
                Success = true
            };

            _mockService
                .Setup(s => s.GetCheckins(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<DateOnly?>(),
                    It.IsAny<DateOnly?>(),
                    It.IsAny<int?>(),
                    It.IsAny<int>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetCheckins(0, 10, null, null, null);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult!.Value as GetCheckInsResponseDTO;
            Assert.That(response, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(response!.TotalRecords, Is.EqualTo(2));
                Assert.That(response.Message, Is.Null);
                Assert.That(response.Success, Is.True);
                Assert.That(response.Data.Count, Is.EqualTo(2));
            });
        }
    }
}