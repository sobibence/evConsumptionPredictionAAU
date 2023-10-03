using System.Net;

namespace EVCP.Api.E2ETests
{
    public class Tests
    {
        [Test]
        public async Task GetWeatherForecast()
        {
            // Arrange
            var client = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:7251/")
            };

            // Act
            var response = await client.GetAsync("/WeatherForecast");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}