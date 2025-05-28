using Android.App;
using Android.App.Job;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using MeTracker.Models;
using MeTracker.Services;
using Location = Android.Locations.Location;

namespace MeTracker.Droid;

[Service(
   Name = "MeTracker.Droid.LocationJobService",
   Permission = "android.permission.BIND_JOB_SERVICE")
]
internal sealed class LocationJobService : JobService, ILocationListener
{
   private static LocationManager? _LocationManager;

   private readonly ILocationRepository _locationRepository =
      IPlatformApplication.Current?.Services.GetService<ILocationRepository>() 
      ?? throw new InvalidOperationException($"No instance for {nameof(ILocationRepository)}");

   public void OnLocationChanged(Location location)
   {
      var newLocation = new LocationEntry(location.Latitude, location.Longitude);
      _locationRepository.SaveAsync(newLocation);
   }

   public void OnProviderDisabled(string provider)
   {
   }

   public void OnProviderEnabled(string provider)
   {
   }

   public void OnStatusChanged(string? provider, [GeneratedEnum] Availability status, Bundle? extras)
   {
   }

   public override bool OnStartJob(JobParameters? jobParameters)
   {
      var status = PermissionStatus.Unknown;

      // ReSharper disable once AsyncApostle.AsyncWait
      Task.Run(
         async () => status = await AppPermissions.CheckRequiredPermissionAsync()
            .ConfigureAwait(true)
      ).Wait();

      if (status != PermissionStatus.Granted)
      {
         return false;
      }

      if (ApplicationContext == null)
      {
         return false;
      }

      _LocationManager = (LocationManager?)ApplicationContext.GetSystemService(LocationService);
      if (_LocationManager != null)
      {
         _LocationManager.RequestLocationUpdates(LocationManager.GpsProvider, 1000L, 0.1f, this);
         return true;
      }

      return false;
   }

   public override bool OnStopJob(JobParameters? jobParameters) => true;
}