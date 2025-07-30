using System.Globalization;
using System.Text.Json;
using Weather.Models;

namespace Weather.Services;

public class OpenWeatherMapWeatherService : IWeatherService
{
   private const string ForecastApiKey = "faf4f5340d3eaec70a66269ed04a4052";
   private readonly HttpClient _httpClient = new();

   public async Task<Forecast> GetForecastAsync(double latitude, double longitude)
   {
      var language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
      var uri =
         $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&units=metric&lang={language}&appid={ForecastApiKey}";

      var result = await _httpClient.GetStringAsync(uri);
      var data = JsonSerializer.Deserialize<WeatherData>(result);
      var forecast = new Forecast
      {
         City = data.City.Name,
         Items = data.List.Select(forecastJson => new ForecastItem
         {
            DateTime = ToDateTime(forecastJson.Dt),
            Temperature = forecastJson.Main.Temp,
            WindSpeed = forecastJson.Wind.Speed,
            Description = forecastJson.Weather.First().Description,
            Icon = $"https://openweathermap.org/img/w/{forecastJson.Weather.First().Icon}.png"
         }).ToList()
      };

      return forecast;
   }

   private static DateTime ToDateTime(double unixTimeStamp)
   {
      var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
      dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
      return dateTime;
   }
}