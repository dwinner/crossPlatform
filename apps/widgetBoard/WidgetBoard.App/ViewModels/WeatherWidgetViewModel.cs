using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WidgetBoard.App.Communication;
using WidgetBoard.App.Services;

namespace WidgetBoard.App.ViewModels;

public partial class WeatherWidgetViewModel : ViewModelBase, IWidgetViewModel
{
   internal const string DisplayName = "Weather";
   private readonly ILocationService _locationService;
   private readonly ISecureStorage _secureStorage;
   private readonly IWeatherForecastService _weatherForecastService;

   [ObservableProperty] private string _iconUrl = string.Empty;
   [ObservableProperty] private State _state;
   [ObservableProperty] private double _temperature;
   [ObservableProperty] private string _weather = string.Empty;

   public WeatherWidgetViewModel(
      IWeatherForecastService weatherForecastService,
      ISecureStorage secureStorage,
      ILocationService locationService)
   {
      _weatherForecastService = weatherForecastService;
      _secureStorage = secureStorage;
      _locationService = locationService;
      Task.Run(async () => await LoadWeather());
   }

   public int Position { get; set; }

   public string Type => DisplayName;

   [RelayCommand]
   public async Task LoadWeather()
   {
      var apiKey = await _secureStorage.GetAsync("OpenWeatherApiToken");

      if (apiKey is null)
      {
         return;
      }

      try
      {
         State = State.Loading;

         var location = await _locationService.GetLocationAsync();
         if (location is null)
         {
            State = State.PermissionError;
            return;
         }

         var latitude = location.Latitude;
         var longitude = location.Longitude;
         var forecast = await _weatherForecastService.GetForecast(latitude, longitude, apiKey);
         if (forecast?.Main is null)
         {
            State = State.Error;
            return;
         }

         Temperature = forecast.Main.Temperature;
         Weather = forecast.Weather.First().Main;
         IconUrl = forecast.Weather.First().IconUrl;
         State = State.Loaded;
      }
      catch (Exception)
      {
         State = State.Error;
      }
   }
}