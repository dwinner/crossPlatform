using Microsoft.Extensions.DependencyInjection;
using Recipes.Client.Core.Features.Favorites;
using Recipes.Client.Core.Features.Ratings;
using Recipes.Client.Core.Features.Recipes;
using Recipes.Client.Repositories.Api;
using Refit;

namespace Recipes.Client.Repositories;

public static class ServiceCollectionExtension
{
   public static IServiceCollection RegisterRepositories(this IServiceCollection self,
      RepositorySettings settings)
   {
      self.AddSingleton(_ => RestService.For<IRatingsApi>(settings.HttpClient));
      self.AddSingleton(_ => RestService.For<IRecipeApi>(settings.HttpClient));
      self.AddSingleton(_ => RestService.For<IFavoritesApi>(settings.HttpClient));

      self.AddTransient<IRatingsRepository, RatingsApiGateway>();
      self.AddTransient<IRecipeRepository, RecipeApiGateway>();
      self.AddTransient<IFavoritesRepository, FavoritesApiGateway>();

      return self;
   }
}