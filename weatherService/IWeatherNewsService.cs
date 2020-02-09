using System.Collections.Generic;
using weatherModel;

namespace weatherService
{
    public interface IWeatherNewsService
    {
        IEnumerable<WeatherNews> GetWeatherNews();
    }
}
