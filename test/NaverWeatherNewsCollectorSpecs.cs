using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using weatherService;

namespace test
{
    [TestClass]
    public class WeatherNewsServiceSpecs
    {
        [TestMethod]
        public void get_weather_news_should_return_collectly()
        {
            // Arrange
            var sut = new WeatherNewsService();

            // Act
            var result = sut.GetWeatherNews();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCountGreaterThan(10);
            result.Any(x =>
                x.BroadcastingCompany != ""
                && x.LinkUrl != ""
                && x.Title != "").Should().BeTrue();
        }
    }
}
