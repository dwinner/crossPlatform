using WidgetBoard.App.Communication;
using WidgetBoard.App.ViewModels;
using WidgetBoard.Tests.Mocks;

//[assembly: UseVerify]

namespace WidgetBoard.SnapshotTests;

public class WeatherWidgetViewModelTests
{
   [Fact]
   public async Task NullLocationResultsInPermissionErrorState()
   {
      var viewModel = new WeatherWidgetViewModel(
         MockWeatherForecastService.ThatReturnsNoForecast(TimeSpan.FromSeconds(5)),
         MockSecureStorage.ThatContains("OpenWeatherApiToken", "SomethingSecure"),
         MockLocationService.ThatReturnsNoLocation(TimeSpan.FromSeconds(2)));

      await viewModel.LoadWeather();
      await Verify(viewModel);
      /*Assert.Equal(State.PermissionError, viewModel.State);
      Assert.Equal(viewModel.Weather, string.Empty);*/
   }

   [Fact]
   public async Task NullForecastResultsInErrorState()
   {
      var viewModel = new WeatherWidgetViewModel(
         MockWeatherForecastService.ThatReturnsNoForecast(TimeSpan.FromSeconds(5)),
         MockSecureStorage.ThatContains("OpenWeatherApiToken", "SomethingSecure"),
         MockLocationService.ThatReturns(new Location(0.0, 0.0), TimeSpan.FromSeconds(2)));

      await viewModel.LoadWeather();
      await Verify(viewModel);
      /*Assert.Equal(State.Error, viewModel.State);
      Assert.Equal(viewModel.Weather, string.Empty);*/
   }

   [Fact]
   public async Task ValidForecastResultsInSuccessfulLoad()
   {
      var weatherForecastService =
         MockWeatherForecastService.ThatReturns(
            new Forecast
            {
               Main = new Main
               {
                  Temperature = 18.0
               },
               Weather =
               [
                  new Weather
                  {
                     Icon = "abc.png",
                     Main = "Sunshine"
                  }
               ]
            },
            TimeSpan.FromSeconds(5));

      var locationService = MockLocationService.ThatReturns(
         new Location(0.0, 0.0),
         TimeSpan.FromSeconds(2));

      var viewModel = new WeatherWidgetViewModel(
         weatherForecastService,
         MockSecureStorage.ThatContains("OpenWeatherApiToken", "SomethingSecure"),
         locationService);

      await viewModel.LoadWeather();
      await Verify(viewModel);
      /*Assert.Equal(State.Loaded, viewModel.State);
      Assert.Equal("Sunshine", viewModel.Weather);*/
   }
}