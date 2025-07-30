using DoToo.Repositories;
using DoToo.ViewModels;
using DoToo.Views;

namespace DoToo;

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
         })
         .RegisterServices()
         .RegisterViewModels()
         .RegisterViews();

      return builder.Build();
   }

   private static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
   {
      mauiAppBuilder.Services.AddSingleton<ITodoItemRepository, TodoItemRepository>();
      return mauiAppBuilder;
   }

   private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
   {
      mauiAppBuilder.Services.AddTransient<MainViewModel>();
      mauiAppBuilder.Services.AddTransient<ItemViewModel>();
      return mauiAppBuilder;
   }

   private static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
   {
      mauiAppBuilder.Services.AddTransient<MainView>();
      mauiAppBuilder.Services.AddTransient<ItemView>();
      return mauiAppBuilder;
   }
}