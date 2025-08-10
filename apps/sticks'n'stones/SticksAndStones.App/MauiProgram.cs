using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SticksAndStones.Services;
using SticksAndStones.ViewModels;
using SticksAndStones.Views;

namespace SticksAndStones.App;

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
            fonts.AddFont("FontAwesome.otf", "FontAwesome");
         })
         .UseMauiCommunityToolkit();

#if DEBUG
      builder.Logging.AddDebug();

      builder.Services.AddLogging(configure => { configure.AddDebug(); });
#endif
      builder.Services.AddSingleton<Settings>();
      builder.Services.AddSingleton<ServiceConnection>();

      builder.Services.AddSingleton<GameService>();

      builder.Services.AddTransient<ConnectViewModel>();
      builder.Services.AddTransient<LobbyViewModel>();
      builder.Services.AddTransient<MatchViewModel>();

      builder.Services.AddTransient<ConnectView>();
      builder.Services.AddTransient<LobbyView>();
      builder.Services.AddTransient<MatchView>();

      return builder.Build();
   }
}