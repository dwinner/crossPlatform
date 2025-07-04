namespace People;

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

      var dbPath = FileAccessHelper.GetLocalFilePath("people.db3");
      builder.Services.AddSingleton(
         svcProvider => ActivatorUtilities.CreateInstance<PersonRepository>(svcProvider, dbPath));

      return builder.Build();
   }
}