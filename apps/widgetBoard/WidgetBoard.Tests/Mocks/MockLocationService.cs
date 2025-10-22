using WidgetBoard.App.Services;

namespace WidgetBoard.Tests.Mocks;

public class MockLocationService : ILocationService
{
   private readonly TimeSpan _delay;
   private readonly Location? _location;

   private MockLocationService(Location? mockLocation, TimeSpan delay)
   {
      _location = mockLocation;
      _delay = delay;
   }

   public async Task<Location?> GetLocationAsync()
   {
      await Task.Delay(_delay);
      return _location;
   }

   public static ILocationService ThatReturns(Location? location, TimeSpan after) =>
      new MockLocationService(location, after);

   public static ILocationService ThatReturnsNoLocation(TimeSpan after) =>
      new MockLocationService(null, after);
}