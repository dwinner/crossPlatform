using Recipes.Shared;
using Refit;

namespace Recipes.Client.Repositories.Api;

/*
 * FIXME: Pay attention that Get paths are hardcoded like in WEB API
 */

public interface IRecipeApi
{
   [Get("/recipe/{recipeId}")]
   Task<ApiResponse<RecipeDetailDto>> GetRecipe(string recipeId);

   [Get("/recipes")]
   Task<ApiResponse<RecipeOverviewItemsDto>> GetRecipes(
      [Header("Accept-Language")] string language, int pageSize = 7,
      int pageIndex = 0);
}