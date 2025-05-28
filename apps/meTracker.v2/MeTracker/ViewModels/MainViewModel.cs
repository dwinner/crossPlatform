using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using MeTracker.Services;
using Point = MeTracker.Models.Point;

namespace MeTracker.ViewModels;

public partial class MainViewModel : ViewModelBase
{
   private readonly ILocationRepository _locationRepository;

   [ObservableProperty] private List<Point> _points;

   public MainViewModel(ILocationRepository locationRepository, ILocationTrackingService locationTrackingService)
   {
      _locationRepository = locationRepository;

      // ReSharper disable once AsyncVoidLambda
      MainThread.BeginInvokeOnMainThread(async () =>
      {
         locationTrackingService.StartTracking();
         await LoadDataAsync().ConfigureAwait(true);
         Debug.WriteLine("Start tracking");
      });
   }

   private async Task LoadDataAsync()
   {
      var locations = await _locationRepository.GetAllAsync()
         .ConfigureAwait(true);
      var points = new List<Point>();
      foreach (var location in locations)
      {
         // If no points exist, create a new one and continue to the next location in the list
         if (!points.Any())
         {
            points.Add(new Point { Location = location });
            continue;
         }

         var pointFound = false;

         // try to find a point for the current location
         foreach (var point in
                  from point in points
                  let distance = Location.CalculateDistance(
                     new Location(point.Location.Latitude, point.Location.Longitude),
                     new Location(location.Latitude, location.Longitude),
                     DistanceUnits.Kilometers)
                  where distance < 0.2
                  select point)
         {
            pointFound = true;
            point.Count++;
            break;
         }

         // if no point is found, add a new Point to the list of points
         if (!pointFound)
         {
            points.Add(new Point { Location = location });
         }

         // Next section of code goes here
         if (!points.Any())
         {
            return;
         }

         var pointMax = points.Select(x => x.Count).Max();
         var pointMin = points.Select(x => x.Count).Min();
         var diff = (float)(pointMax - pointMin);

         // Last section of code goes here
         foreach (var point in points)
         {
            var heat = 2f / 3f - point.Count / diff;
            point.Heat = Color.FromHsla(heat, 1, 0.5);
         }
      }

      Points = points;
   }
}