using MHCI.Application.Models;
using MHCI.Application.Services;
using MHCI.Domain.Entities;
using MHCI.Domain.Repositories;
using Moq;
namespace MHCI.Tests.Services
{
    [TestFixture]
    public class CheckInServiceTests
    {
        private Mock<ICheckInRepository> _mockRepository;
        private CheckInService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<ICheckInRepository>();
            _service = new CheckInService(_mockRepository.Object);
        }

        [Test]
        public async Task CreateCheckin_ValidModel_ReturnsTrue()
        {
            // Arrange
            var model = new CheckInModel
            {
                UserId = 1,
                Mood = 4,
                Notes = "Feeling good"
            };

            _mockRepository
                .Setup(r => r.CreateCheckIn(It.IsAny<CheckIn>()))
                .ReturnsAsync(true);

            // Act
            var result = await _service.CreateCheckin(model);

            // Assert
            Assert.That(result, Is.True);

            _mockRepository.Verify(r => r.CreateCheckIn(It.Is<CheckIn>(c =>
                c.UserId == model.UserId &&
                c.Mood == model.Mood &&
                c.Notes == model.Notes &&
                c.CreatedAt == DateOnly.ParseExact(DateTime.Now.ToString("yyyy-MM-dd"), "yyyy-MM-dd")
            )), Times.Once);
        }

        [Test]
        public async Task CreateCheckin_InvalidModel_ReturnsFalse()
        {
            // Arrange
            var model = new CheckInModel
            {
                UserId = 0,   // invalid
                Mood = 10,    // invalid
                Notes = ""    // invalid
            };

            // Act
            var result = await _service.CreateCheckin(model);

            // Assert
            Assert.That(result, Is.False);

            // Repository should never be called
            _mockRepository.Verify(r => r.CreateCheckIn(It.IsAny<CheckIn>()), Times.Never);
        }
    }
}