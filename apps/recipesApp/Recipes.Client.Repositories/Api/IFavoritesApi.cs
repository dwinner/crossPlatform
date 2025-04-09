using Recipes.Client.Core;
using Recipes.Shared;
using Refit;

namespace Recipes.Client.Repositories.Api;

/*
 * FIXME: Pay attention that Http verbs' paths are hardcoded like in WEB API
 */

public interface IFavoritesApi
{
   [Get("/users/{userId}/favorites")]
   Task<ApiResponse<string[]>> GetFavorites(string userId);

   [Post("/users/{userId}/favorites")]
   Task<ApiResponse<Nothing>> AddFavorite(string userId, FavoriteDto favorite);

   [Delete("/users/{userId}/favorites/{recipeId}")]
   Task<ApiResponse<Nothing>> DeleteFavorite(string userId, string recipeId);
}