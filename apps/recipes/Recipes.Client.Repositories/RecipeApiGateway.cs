using Recipes.Client.Core;
using Recipes.Client.Core.Features.Recipes;
using Recipes.Client.Repositories.Api;
using Recipes.L10N;
using static Recipes.Client.Repositories.Mappers.RecipeMapper;

namespace Recipes.Client.Repositories;

internal class RecipeApiGateway : ApiGateway, IRecipeRepository
{
   private readonly IRecipeApi _api;
   private readonly ILocalizationManager _l10NManager;

   public RecipeApiGateway(IRecipeApi api, ILocalizationManager l10NManager)
   {
      _api = api;
      _l10NManager = l10NManager;
   }

   public Task<Result<LoadRecipesResponse>> LoadRecipes(int pageSize, int page)
      => InvokeAndMap(
         _api.GetRecipes(_l10NManager.GetUserCulture().Name, pageSize, page),
         MapRecipesOverview);

   public Task<Result<RecipeDetail>> LoadRecipe(string id)
      => InvokeAndMap(_api.GetRecipe(id), MapRecipe);
}