using CoreLocation;
using MeTracker.Models;
using MeTracker.Services;

namespace MeTracker.iOS;

internal sealed class IosLocationTracking(ILocationRepository locationRepository) : ILocationTrackingService
{
   private CLLocationManager _locationManager = null!;

   public void StartTracking()
   {
      _locationManager = new CLLocationManager
      {
         PausesLocationUpdatesAutomatically = false,
         AllowsBackgroundLocationUpdates = true,
         DesiredAccuracy = CLLocation.AccuracyBestForNavigation
      };

      _locationManager.LocationsUpdated += async (_, e) =>
      {
         var lastLocation = e.Locations.Last();
         var newLocation = new LocationEntry(
            lastLocation.Coordinate.Latitude,
            lastLocation.Coordinate.Longitude
         );

         await locationRepository.SaveAsync(newLocation).ConfigureAwait(true);
      };

      _locationManager.RequestAlwaysAuthorization();
      _locationManager.StartUpdatingLocation();
   }
}