using MeTracker.Services;

namespace MeTracker.Droid;

public static class MauiProgram
{
   public static MauiApp CreateMauiApp()
   {
      var builder = MauiApp.CreateBuilder();
      builder.Services.AddSingleton<ILocationTrackingService, DroidLocationTracking>();
      builder.UseSharedMauiApp();

      return builder.Build();
   }
}