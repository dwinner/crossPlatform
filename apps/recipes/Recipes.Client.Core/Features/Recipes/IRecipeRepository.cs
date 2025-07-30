namespace Recipes.Client.Core.Features.Recipes;

public interface IRecipeRepository
{
   protected const int DefaultPageSize = 7;
   protected const int DefaultPageOffset = 0;

   Task<Result<LoadRecipesResponse>> LoadRecipes(int pageSize = DefaultPageSize, int page = DefaultPageOffset);

   Task<Result<RecipeDetail>> LoadRecipe(string id);
}