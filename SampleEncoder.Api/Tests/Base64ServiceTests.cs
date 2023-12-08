using System.Text;
using SampleEncoder.Api.Models;
using Xunit;

namespace SampleEncoder.Api.Tests
{
    public class Base64ServiceTests
    {
        [Fact]
        public async Task EncodeToBase64Async_ValidInput_ReturnsBase64StringAndResponseItems()
        {
            // Arrange
            var base64Service = new Base64Service();
            var inputText = "Hello, World!";

            // Act
            var result = await base64Service.EncodeToBase64Async(inputText);

            // Assert
            // Check that the result is a valid Base64 string
            Assert.NotEmpty(result);
            Assert.Equal(Convert.ToBase64String(Encoding.UTF8.GetBytes(inputText)), result);

            // Check that responseItems were populated during encoding
            Assert.NotNull(base64Service.GetResponseItems());
            Assert.NotEmpty(base64Service.GetResponseItems());

            // Verify that each character in the Base64 string corresponds to a ResponseItem
            foreach (char character in result)
            {
                Assert.Contains(new ResponseItem { Character = character.ToString() }, base64Service.GetResponseItems());
            }
        }
    }

    // Extension method to get ResponseItems from Base64Service (for testing purposes)
    public static class Base64ServiceExtensions
    {
        public static List<ResponseItem> GetResponseItems(this Base64Service base64Service)
        {
            return base64Service.GetType()
                .GetField("responseItems", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .GetValue(base64Service) as List<ResponseItem>;
        }
    }
}
