using Gallery.Core.Services;
using Gallery.Core.ViewModels;
using Gallery.Core.Views;
using Microsoft.Extensions.Logging;

namespace Gallery.Core;

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

      var svc = builder.Services;

      svc.AddTransient<ILocalStorage>(_ => new MauiLocalStorage());

      svc.AddTransient<MainViewModel>();
      svc.AddTransient<GalleryViewModel>();

      svc.AddTransient<MainView>();
      svc.AddTransient<GalleryView>();

      return builder;
   }
}