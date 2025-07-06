using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;

namespace c2_DecoupleViewAndViewModel;

public static class MauiProgram
{
   public static MauiApp CreateMauiApp()
   {
      var builder = MauiApp.CreateBuilder();
      builder
         .UseMauiApp<App>()
         .UseMauiCommunityToolkit()
         .ConfigureFonts(fonts =>
         {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
         });

#if DEBUG
      builder.Logging.AddDebug();
#endif

      builder.Services.AddSingleton<IMessenger>(_ => WeakReferenceMessenger.Default);
      builder.Services.AddTransient<MyViewModel>();
      builder.Services.AddTransient<MainPage>();

      return builder.Build();
   }
}