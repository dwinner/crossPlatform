using System.Text;
using System.Text.Json;
using Recipes.Shared;

namespace Recipes.Web.Api;

public class RecipeService
{
   public RecipeDetailDto? LoadRecipe(string id)
   {
      var recipeDetails = ReadRecipeDetailsFromStream();
      return recipeDetails.SingleOrDefault(recipeDetail => recipeDetail.Id == id);
   }

   public RecipeOverviewItemsDto LoadRecipes(int pageSize = 7, int pageIndex = 0)
   {
      var recipeDetails = ReadRecipeDetailsFromStream().ToList();
      var result = new RecipeOverviewItemsDto(recipeDetails.Count, pageSize, pageIndex,
         recipeDetails
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .Select(recipeDetail =>
               new RecipeOverviewItemDto(recipeDetail.Id, recipeDetail.Name, recipeDetail.Image))
            .ToArray());

      return result;
   }

   private static RecipeDetailDto[] ReadRecipeDetailsFromStream()
   {
      string json;
      const string jsonFile = "recipedetails.json";
      using (var reader = new StreamReader(jsonFile, Encoding.UTF8))
      {
         json = reader.ReadToEnd();
      }

      return JsonSerializer.Deserialize<RecipeDetailDto[]>(json) ?? [];
   }
}