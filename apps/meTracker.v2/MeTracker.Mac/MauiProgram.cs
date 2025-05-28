using MeTracker.Services;

namespace MeTracker.Mac;

public static class MauiProgram
{
   public static MauiApp CreateMauiApp()
   {
      var builder = MauiApp.CreateBuilder();
      builder.Services.AddSingleton<ILocationTrackingService, MacLocationTracking>();
      builder.UseSharedMauiApp();

      return builder.Build();
   }
}