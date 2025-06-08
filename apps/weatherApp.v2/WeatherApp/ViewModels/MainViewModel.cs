using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.ViewModels;

public partial class MainViewModel : ViewModel
{
   private readonly IWeatherService _weatherService;

   [ObservableProperty] private string _city;

   [ObservableProperty] private ObservableCollection<ForecastGroup> _days;

   [ObservableProperty] private bool _isRefreshing;

   [RelayCommand]
   public async Task RefreshAsync()
   {
      await LoadDataAsync();
   }

   public MainViewModel(IWeatherService weatherService)
   {
      _weatherService = weatherService;
   }

   public async Task LoadDataAsync()
   {
      try
      {
         IsRefreshing = true;
         var status = await AppPermissions.CheckAndRequestRequiredPermissionAsync().ConfigureAwait(true);
         if (status == PermissionStatus.Granted)
         {
            var location = await Geolocation.GetLastKnownLocationAsync() ??
                           await Geolocation.GetLocationAsync();
            var forecast = await _weatherService.GetForecastAsync(location.Latitude, location.Longitude);
            var itemGroups = new List<ForecastGroup>();
            foreach (var item in forecast.Items)
            {
               if (itemGroups.Count == 0)
               {
                  itemGroups.Add(
                     new ForecastGroup(new List<ForecastItem> { item })
                     {
                        Date = item.DateTime.Date
                     }
                  );

                  continue;
               }

               var group = itemGroups.SingleOrDefault(x => x.Date == item.DateTime.Date);
               if (group == null)
               {
                  itemGroups.Add(
                     new ForecastGroup(new List<ForecastItem> { item })
                     {
                        Date = item.DateTime.Date
                     }
                  );

                  continue;
               }

               group.Items.Add(item);
            }

            Days = new ObservableCollection<ForecastGroup>(itemGroups);
            City = forecast.City;
         }
      }
      finally
      {
         IsRefreshing = false;
      }
   }
}