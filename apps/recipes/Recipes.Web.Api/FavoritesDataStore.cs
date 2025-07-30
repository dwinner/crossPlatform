using Recipes.Shared;

namespace Recipes.Web.Api;

internal static class FavoritesDataStore
{
   // PERF: Not fast enough container to search
   private static readonly List<string> Favorites = [];

   internal static string[] GetFavorites(string userId) => Favorites.ToArray();

   internal static void StoreFavorite(string userId, FavoriteDto favorite)
   {
      if (Favorites.Contains(favorite.RecipeId))
      {
         return;
      }

      Favorites.Add(favorite.RecipeId);
   }

   internal static void DeleteFavorite(string userId, string recipeId)
   {
      Favorites.Remove(recipeId);
   }
}