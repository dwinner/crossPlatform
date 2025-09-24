namespace WidgetBoard.App.Services;

public partial class PlatformLocationService
{
   public Task<Location?> GetLocationAsync() =>
      Task.FromResult<Location?>(new Location(37.419857, -122.078827));
}