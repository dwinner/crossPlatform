using Microsoft.Extensions.Logging;
using News.Services;
using News.ViewModels;
using News.Views;

namespace News;

public static class MauiProgram
{
   public static MauiApp CreateMauiApp()
   {
      var builder = MauiApp.CreateBuilder();
      builder
         .UseMauiApp<App>()
         .ConfigureFonts(fonts =>
         {
            fonts.AddFont("FontAwesome.otf", "FontAwesome");
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
         })
         .RegisterAppTypes();

#if DEBUG
      builder.Logging.AddDebug();
#endif

      return builder.Build();
   }

   public static MauiAppBuilder RegisterAppTypes(this MauiAppBuilder self)
   {
      var svc = self.Services;

      // Services
      svc.AddSingleton<INewsService>(_ => new NewsService());
      svc.AddSingleton<INavigate>(_ => new Navigator());

      // ViewModels
      svc.AddTransient<HeadlinesViewModel>();

      // Views
      svc.AddTransient<AboutView>();
      svc.AddTransient<ArticleView>();
      svc.AddTransient<HeadlinesView>();

      return self;
   }
}