﻿using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Recipes.Client.Core.Features.Favorites;
using Recipes.Client.Core.Features.Ratings;
using Recipes.Client.Core.Features.Recipes;
using Recipes.Client.Core.Navigation;
using Recipes.Client.Core.Services;
using Recipes.Client.Core.ViewModels;
using Recipes.Client.Repositories;
using Recipes.L10N;
using Recipes.L10N.Maui;
using Recipes.Mobile.Misc;
using Recipes.Mobile.Navigation;
using Recipes.Mobile.Pages;
using Recipes.Mobile.Resources.Strings;
using Recipes.Mobile.Services;

namespace Recipes.Mobile;

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
            fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIconsRegular");
         });

      builder.Services.AddSingleton<App>();
      builder.Services.AddTransient<RecipesOverviewPage>();
      builder.Services.AddTransient<RecipesOverviewViewModel>();
      builder.Services.AddTransient<RecipeDetailPage>();
      builder.Services.AddTransient<RecipeDetailViewModel>();
      builder.Services.AddTransient<RecipeRatingDetailPage>();
      builder.Services.AddTransient<RecipeRatingsDetailViewModel>();
      builder.Services.AddTransient<SettingsPage>();
      builder.Services.AddTransient<SettingsViewModel>();
      builder.Services.AddTransient<PickLanguagePage>();
      builder.Services.AddTransient<PickLanguageViewModel>();
      builder.Services.AddTransient<AddRatingPage>();
      builder.Services.AddTransient<AddRatingViewModel>();
      builder.Services.AddSingleton<IFavoritesService, FavoritesService>();
      builder.Services.AddSingleton<IDialogService, DialogService>();
      builder.Services.AddTransient<IRatingsService, RatingsService>();
      builder.Services.AddTransient<IRecipeService, RecipeService>();
      builder.Services.AddTransient<LoginPageViewModel>();
      builder.Services.AddSingleton<NavigationService>();
      builder.Services.AddSingleton<INavigationService>(
         svcProvider => svcProvider.GetRequiredService<NavigationService>());
      builder.Services.AddSingleton<INavigationInterceptor>(
         svcProvider => svcProvider.GetRequiredService<NavigationService>());
      builder.Services.AddSingleton<ILocalizationManager, LocalizationManager>();
      var resources = new LocalizedResourcesProvider(AppResources.ResourceManager);
      builder.Services.AddSingleton<ILocalizedResourcesProvider>(resources);

      Routing.RegisterRoute("Overview", typeof(RecipesOverviewPage));
      Routing.RegisterRoute("RecipeDetail", typeof(RecipeDetailPage));
      Routing.RegisterRoute("RecipeRating", typeof(RecipeRatingDetailPage));
      Routing.RegisterRoute("PickLanguagePage", typeof(PickLanguagePage));
      Routing.RegisterRoute("SettingsPage", typeof(SettingsPage));
      Routing.RegisterRoute("AddRating", typeof(AddRatingPage));

      var baseAddress = DeviceInfo.Platform == DevicePlatform.Android
         ? "https://10.0.2.2:7037"
         : "https://localhost:44359/";
      var httpClient = HttpClientHelper.GetPlatformHttpClient(baseAddress);
      builder.Services.RegisterRepositories(new RepositorySettings(httpClient));

#if DEBUG
      builder.Logging.AddDebug();
#endif

      return builder.Build();
   }
}