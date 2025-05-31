using Gallery.Core;
using Gallery.Core.Services;

namespace Gallery.Droid;

public static class MauiProgram
{
   public static MauiApp CreateMauiApp()
   {
      var builder = MauiApp.CreateBuilder();
      builder.Services.AddSingleton<IPhotoImporter>(_ => new DroidPhotoImporter());
      builder.UseSharedMauiApp();

      return builder.Build();
   }
}