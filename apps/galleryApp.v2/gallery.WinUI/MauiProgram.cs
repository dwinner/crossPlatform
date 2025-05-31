using Gallery.Core;
using Gallery.Core.Services;
using gallery.WinUI;

namespace Gallery.WinUI;

public static class MauiProgram
{
   public static MauiApp CreateMauiApp()
   {
      var builder = MauiApp.CreateBuilder();
      builder.Services.AddSingleton<IPhotoImporter>(_ => new WinUiPhotoImporter());
      builder.UseSharedMauiApp();

      return builder.Build();
   }
}