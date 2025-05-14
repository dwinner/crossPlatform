using Android;
using Android.App;
using Android.OS;

[assembly: UsesPermission(Manifest.Permission.ReadMediaImages)]
[assembly: UsesPermission(Manifest.Permission.ReadExternalStorage, MaxSdkVersion = 32)]

namespace Gallery.Droid;

// APPLY: Unclear how to attach permissions via separated project

internal sealed class AppPermission : Permissions.Photos
{
   public override (string androidPermission, bool isRuntime)[] RequiredPermissions
   {
      get
      {
         List<(string androidPermission, bool isRuntime)> permissions =
         [
            Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu
               ? (Manifest.Permission.ReadMediaImages, true)
               : (Manifest.Permission.ReadExternalStorage, true)
         ];

         return permissions.ToArray();
      }
   }
}