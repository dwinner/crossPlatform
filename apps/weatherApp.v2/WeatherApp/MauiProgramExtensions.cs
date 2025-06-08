using Microsoft.Extensions.Logging;
using WeatherApp.Services;
using WeatherApp.ViewModels;
using WeatherApp.Views;
using MobileMainView = WeatherApp.Views.Mobile.MainView;
using DesktopMainView = WeatherApp.Views.Desktop.MainView;

namespace WeatherApp;

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

      builder.Services.AddSingleton<IWeatherService, OpenWeatherMapWeatherService>();
      builder.Services.AddTransient<MainViewModel, MainViewModel>();
      if (DeviceInfo.Idiom == DeviceIdiom.Phone)
      {
         builder.Services.AddTransient<IMainView, MobileMainView>();
      }
      else
      {
         builder.Services.AddTransient<IMainView, DesktopMainView>();
      }

      return builder;
   }
}