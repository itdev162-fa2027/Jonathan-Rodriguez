using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    private readonly DataContext _context;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index =>
        {
            int temperatureC = Random.Shared.Next(-20, 55);

            return new WeatherForecast
            {
                Id = 0,
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = temperatureC,
                TemperatureF = 32 + (int)(temperatureC / 0.5556),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            };
        })
        .ToArray();
    }

    public ActionResult<WeatherForecast> Create()
    {
        Console.WriteLine($"Database path: {_context.DbPath}");
        Console.WriteLine("Insert a new WeatherForecast");

        var forecast = new WeatherForecast()
        {
            Date = new DateOnly(),
            TemperatureC = 75,
            Summary = "Warm"
        };

        _context.WeatherForecasts.Add(forecast);
        var success = _context.SaveChanges() > 0;

        if (success)
        {
            return forecast;
        }

        throw new Exception("Error creating WeatherForecast");
    }
}