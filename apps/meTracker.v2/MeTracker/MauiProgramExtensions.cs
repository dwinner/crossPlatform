using MeTracker.Impl;
using MeTracker.Services;
using MeTracker.ViewModels;
using MeTracker.Views;
using Microsoft.Extensions.Logging;

namespace MeTracker;

public static class MauiProgramExtensions
{
   public static MauiAppBuilder UseSharedMauiApp(this MauiAppBuilder builder)
   {
      builder
         .UseMauiApp<App>()
         .ConfigureFonts(fonts =>
         {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
         });

#if DEBUG
      builder.Logging.AddDebug();
#endif

      var services = builder.Services;
      services.AddSingleton<ILocationRepository, SqLiteLocationRepository>();
      services.AddTransient(typeof(MainViewModel));
      services.AddTransient(typeof(MainView));

      return builder;
   }
}