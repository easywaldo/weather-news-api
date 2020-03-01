using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using weatherModel;
using weatherService;

namespace hellodotnetcore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherNewsService _weatherNewsService;
        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IWeatherNewsService weatherNewsService)
        {
            _logger = logger;
            _weatherNewsService = weatherNewsService;
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        [HttpGet]
        [Route("GetList")]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [EnableCors]
        [HttpGet]
        [Route("GetWeatherNews")]
        public IEnumerable<WeatherNews> GetWeatherNews()
        {
            var userPrincipal = HttpContext.Request.HttpContext.User.Claims;

            // Look for the LastChanged claim.
            var lastChanged = (from c in userPrincipal
                               where c.Type == "LastChanged"
                               select c.Value).FirstOrDefault();

            //if (string.IsNullOrEmpty(lastChanged))
            //{
            //    //HttpContext.Request.HttpContext.Abort();
            //    return Enumerable.Empty<WeatherNews>();
            //}

            //HttpContext.Response.StatusCode = 200;
            return _weatherNewsService.GetWeatherNews();
        }
    }
}
