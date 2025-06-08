using WeatherApp.Models;

namespace WeatherApp.Services;

public interface IWeatherService
{
   Task<Forecast> GetForecastAsync(double latitude, double longitude);
}