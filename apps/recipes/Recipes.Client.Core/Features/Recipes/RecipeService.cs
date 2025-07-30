namespace Recipes.Client.Core.Features.Recipes;

public sealed class RecipeService : IRecipeService
{
   private readonly IRecipeRepository _recipeRepository;

   public RecipeService(IRecipeRepository recipeRepository) => _recipeRepository = recipeRepository;

   public Task<Result<LoadRecipesResponse>> LoadRecipes(int pageSize = 7, int page = 0)
      => _recipeRepository.LoadRecipes(pageSize, page);

   public Task<Result<RecipeDetail>> LoadRecipe(string id)
      => _recipeRepository.LoadRecipe(id);
}