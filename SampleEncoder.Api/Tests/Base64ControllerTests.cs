using Microsoft.AspNetCore.Mvc;
using Moq;
using SampleEncoder.Api.Models;
using Xunit;

namespace SampleEncoder.Api.Tests
{
    public class Base64ControllerTests
    {
        [Fact]
        public async Task EncodeToBase64_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var mockBase64Service = new Mock<IBase64Service>();
            var mockLogger = new Mock<ILogger<Base64Controller>>();
            var controller = new Base64Controller(mockBase64Service.Object, mockLogger.Object);
            var inputText = "Hello, World!";
            var request = new TextEncodeRequest { Text = inputText };

            // Assume that the encoding service works correctly
            mockBase64Service.Setup(service => service.EncodeToBase64Async(inputText))
                            .ReturnsAsync("SGVsbG8sIFdvcmxkIQ==");

            // Act
            var result = await controller.EncodeToBase64(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsType<JsonResult>(okResult.Value);
            dynamic data = resultValue.Value;
            Assert.Equal("SGVsbG8sIFdvcmxkIQ==", data.Base64Result);
        }


        [Fact]
        public void CancelEncoding_ReturnsOkResult()
        {
            // Arrange
            var mockBase64Service = new Mock<IBase64Service>();
            var mockLogger = new Mock<ILogger<Base64Controller>>();
            var controller = new Base64Controller(mockBase64Service.Object, mockLogger.Object);

            // Act
            var result = controller.CancelEncoding();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Encoding process canceled.", okResult.Value);
        }

        // Add more tests for different scenarios, error cases, etc.
    }
}
