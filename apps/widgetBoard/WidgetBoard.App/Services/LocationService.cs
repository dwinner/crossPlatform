namespace WidgetBoard.App.Services;

internal sealed class LocationService(IGeolocation geolocation) : ILocationService
{
   public async Task<Location?> GetLocationAsync() =>
      await MainThread.InvokeOnMainThreadAsync(async () =>
      {
         var status = await CheckAndRequestLocationPermission();
         return status != PermissionStatus.Granted
            ? null
            : await geolocation.GetLocationAsync();
      });

   private static async Task<PermissionStatus> CheckAndRequestLocationPermission()
   {
      var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
      if (status == PermissionStatus.Granted)
      {
         return status;
      }

      if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
      {
         // Prompt the user to turn on in settings
         // On iOS once a permission has been denied it may not be requested again from the application
         return status;
      }

      if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
      {
         // Prompt the user with additional information as to why the permission is needed
      }

      status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

      return status;
   }
}