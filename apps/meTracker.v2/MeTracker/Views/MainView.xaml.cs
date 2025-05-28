using MeTracker.ViewModels;
using Microsoft.Maui.Maps;

namespace MeTracker.Views;

public partial class MainView
{
   public MainView(MainViewModel viewModel)
   {
      InitializeComponent();
      BindingContext = viewModel;

      // ReSharper disable once AsyncVoidLambda
      MainThread.BeginInvokeOnMainThread(async () =>
      {
         var status = await AppPermissions.CheckAndRequestRequiredPermissionAsync()
            .ConfigureAwait(true);
         if (status == PermissionStatus.Granted)
         {
            var location = await Geolocation.GetLastKnownLocationAsync().ConfigureAwait(true)
                           ?? await Geolocation.GetLocationAsync().ConfigureAwait(true);
            if (location != null)
            {
               customMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                  location,
                  Distance.FromKilometers(5))
               );
            }
         }
      });
   }
}