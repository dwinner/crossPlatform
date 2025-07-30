using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Weather.Models;
using Weather.Services;

namespace Weather.ViewModels;

public partial class MainViewModel(IWeatherService weatherService) : ViewModel
{
   [ObservableProperty] private string _city;

   [ObservableProperty] private ObservableCollection<ForecastGroup> _days;

   [ObservableProperty] private bool _isRefreshing;

   [RelayCommand]
   public async Task RefreshAsync()
   {
      await LoadDataAsync();
   }

   public async Task LoadDataAsync()
   {
      try
      {
         IsRefreshing = true;

         var status = await AppPermissions.CheckAndRequestRequiredPermissionAsync();
         if (status == PermissionStatus.Granted)
         {
            var location = await Geolocation.GetLastKnownLocationAsync()
                           ?? await Geolocation.GetLocationAsync();
            if (location == null)
            {
               return;
            }

            var forecast = await weatherService.GetForecastAsync(location.Latitude, location.Longitude);
            var itemGroups = GetForecastGroup(forecast);
            Days = new ObservableCollection<ForecastGroup>(itemGroups);
            City = forecast.City;
         }
      }
      finally
      {
         IsRefreshing = false;
      }
   }

   private static List<ForecastGroup> GetForecastGroup(Forecast forecast)
   {
      var itemGroups = new List<ForecastGroup>();
      foreach (var forecastItem in forecast.Items)
      {
         if (itemGroups.Count == 0)
         {
            itemGroups.Add(new ForecastGroup(new List<ForecastItem> { forecastItem })
            {
               Date = forecastItem.DateTime.Date
            });

            continue;
         }

         var group = itemGroups.SingleOrDefault(forecastGrp => forecastGrp.Date == forecastItem.DateTime.Date);
         if (group == null)
         {
            itemGroups.Add(new ForecastGroup(new List<ForecastItem> { forecastItem })
            {
               Date = forecastItem.DateTime.Date
            });

            continue;
         }

         group.Items.Add(forecastItem);
      }

      return itemGroups;
   }
}