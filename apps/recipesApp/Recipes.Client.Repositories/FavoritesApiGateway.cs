using Recipes.Client.Core;
using Recipes.Client.Core.Features.Favorites;
using Recipes.Client.Repositories.Api;
using Recipes.Shared;

namespace Recipes.Client.Repositories;

internal class FavoritesApiGateway : ApiGateway, IFavoritesRepository
{
   private readonly IFavoritesApi _api;

   public FavoritesApiGateway(IFavoritesApi api) => _api = api;

   public Task<Result<Nothing>> Add(string userId, string id)
      => InvokeAndMap(_api.AddFavorite(userId, new FavoriteDto(id)));

   public Task<Result<string[]>> LoadFavorites(string userId)
      => InvokeAndMap(_api.GetFavorites(userId));

   public Task<Result<Nothing>> Remove(string userId, string recipeId)
      => InvokeAndMap(_api.DeleteFavorite(userId, recipeId));
}