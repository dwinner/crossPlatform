using System.Diagnostics;

namespace Gallery.Core;

public static class AppPermissions
{
   public static async Task<PermissionStatus> CheckRequiredPermissionAsync()
   {
      var permissionStatus = await Permissions.CheckStatusAsync<AppPermission>()
         .ConfigureAwait(true);
      return permissionStatus;
   }

   public static async Task<PermissionStatus> CheckAndRequestRequiredPermissionAsync()
   {
      var status = await Permissions.CheckStatusAsync<AppPermission>()
         .ConfigureAwait(true);

      var mainPage = Application.Current?.Windows[0].Page;
      Debug.Assert(mainPage is not null);

      switch (status)
      {
         case PermissionStatus.Granted:
            return status;

         case PermissionStatus.Denied when DeviceInfo.Platform == DevicePlatform.iOS:
            // Prompt the user to turn on in settings
            // On iOS once a permission has been denied it may not be requested again from the application
            await mainPage.DisplayAlert(
               "Required App Permissions",
               "Please enable all permissions in Settings for this App, it is useless without them.",
               "Ok"
            ).ConfigureAwait(true);
            break;
      }

      if (Permissions.ShouldShowRationale<AppPermission>())
      {
         // Prompt the user with additional information as to why the permission is needed
         await mainPage.DisplayAlert(
            "Required App Permissions",
            "This is a Photo gallery app, without these permissions it is useless.",
            "Ok"
         ).ConfigureAwait(true);
      }

      status = await MainThread.InvokeOnMainThreadAsync(Permissions.RequestAsync<AppPermission>)
         .ConfigureAwait(true);

      return status;
   }

   internal class AppPermission : Permissions.Photos
   {
   }
}