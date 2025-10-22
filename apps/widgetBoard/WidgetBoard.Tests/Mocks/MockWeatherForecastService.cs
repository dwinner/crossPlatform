using WidgetBoard.App.Communication;

namespace WidgetBoard.Tests.Mocks;

public class MockWeatherForecastService : IWeatherForecastService
{
   private readonly TimeSpan _delay;
   private readonly Forecast? _forecast;

   private MockWeatherForecastService(Forecast? forecast, TimeSpan delay)
   {
      _forecast = forecast;
      _delay = delay;
   }

   public async Task<Forecast?> GetForecast(double latitude, double longitude, string apiKey)
   {
      await Task.Delay(_delay);
      return _forecast;
   }

   public static IWeatherForecastService ThatReturns(Forecast? forecast, TimeSpan after) =>
      new MockWeatherForecastService(forecast, after);

   public static IWeatherForecastService ThatReturnsNoForecast(TimeSpan after) =>
      new MockWeatherForecastService(null, after);
}