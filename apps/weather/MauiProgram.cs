using Microsoft.Extensions.Logging;
using Weather.Services;
using Weather.ViewModels;
using Weather.Views;
using MobileMainView = Weather.Views.Mobile.MainView;
using DesktopMainView = Weather.Views.Desktop.MainView;

namespace Weather;

public static class MauiProgram
{
   public static MauiApp CreateMauiApp()
   {
      var builder = MauiApp.CreateBuilder();
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

      return builder.Build();
   }
}