namespace Weather;

internal class AppPermissions
{
   public static async Task<PermissionStatus> CheckRequiredPermissionAsync() =>
      await Permissions.CheckStatusAsync<AppPermission>();

   public static async Task<PermissionStatus> CheckAndRequestRequiredPermissionAsync()
   {
      var status = await Permissions.CheckStatusAsync<AppPermission>();
      var mainPage = Application.Current?.Windows[0].Page
                     ?? throw new InvalidOperationException("No active page");
      switch (status)
      {
         case PermissionStatus.Granted:
            return status;
         case PermissionStatus.Denied when DeviceInfo.Platform == DevicePlatform.iOS:
            // Prompt the user to turn on in settings
            // On iOS once a permission has been denied it may not be requested again from the application
            await mainPage.DisplayAlert("Required App Permissions",
               "Please enable all permissions in Settings for this App, it is useless without them.", "Ok");
            break;
      }

      if (Permissions.ShouldShowRationale<AppPermission>())
      {
         // Prompt the user with additional information as to why the permission is needed
         await mainPage.DisplayAlert("Required App Permissions",
            "This is a location based app, without these permissions it is useless.", "Ok");
      }

      status = await MainThread.InvokeOnMainThreadAsync(Permissions.RequestAsync<AppPermission>);
      return status;
   }

   internal class AppPermission : Permissions.LocationWhenInUse
   {
   }
}