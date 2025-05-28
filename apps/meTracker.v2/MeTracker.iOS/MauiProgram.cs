using MeTracker.Services;

namespace MeTracker.iOS;

public static class MauiProgram
{
   public static MauiApp CreateMauiApp()
   {
      var builder = MauiApp.CreateBuilder();
      builder.Services.AddSingleton<ILocationTrackingService, IosLocationTracking>();
      builder.UseSharedMauiApp();

      return builder.Build();
   }
}