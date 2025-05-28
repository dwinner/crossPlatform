using MeTracker.Models;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;
using Point = MeTracker.Models.Point;

namespace MeTracker.Controls;

public class CustomMap : Map
{
   public static readonly BindableProperty PointsProperty = BindableProperty.Create(
      nameof(Points),
      typeof(List<LocationEntry>),
      typeof(CustomMap),
      new List<LocationEntry>(),
      propertyChanged: OnPointsChanged
   );

   public CustomMap()
   {
      IsScrollEnabled = true;
      IsShowingUser = true;
   }

   public List<LocationEntry> Points
   {
      get => GetValue(PointsProperty) as List<LocationEntry>
             ?? throw new InvalidOperationException("Points are null");
      set => SetValue(PointsProperty, value);
   }

   private static void OnPointsChanged(BindableObject bindable, object? oldValue, object? newValue)
   {
      if (bindable is not Map map)
      {
         return;
      }

      if (newValue is not List<Point> points)
      {
         return;
      }

      foreach (var circle in points.Select(point => new Circle
               {
                  Center = new Location(point.Location.Latitude, point.Location.Longitude),
                  Radius = new Distance(200),
                  StrokeColor = Color.FromArgb("#88FF0000"),
                  StrokeWidth = 0,
                  FillColor = point.Heat
               }))
      {
         map.MapElements.Add(circle);
      }
   }
}