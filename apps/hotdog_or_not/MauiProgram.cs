using HotdogOrNot.ViewModels;
using HotdogOrNot.Views;
using Microsoft.Extensions.Logging;

namespace HotdogOrNot;

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

      builder.Services.AddTransient<MainView>();
      builder.Services.AddTransient<ResultView>();
      builder.Services.AddTransient<MainViewModel>();
      builder.Services.AddTransient<ResultViewModel>();

      return builder.Build();
   }
}