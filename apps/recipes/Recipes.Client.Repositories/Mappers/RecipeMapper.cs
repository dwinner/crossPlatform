using Recipes.Client.Core.Features.Recipes;
using Recipes.Shared;

namespace Recipes.Client.Repositories.Mappers;

/*
 * TOREFACTOR: Such mapping can be done via overloaded cast operators or Automapper lib.
 */

internal static class RecipeMapper
{
   internal static LoadRecipesResponse MapRecipesOverview(RecipeOverviewItemsDto result)
      => new(
         result.TotalItems,
         result.PageIndex,
         result.PageSize,
         result.Recipes.Select(MapRecipesOverviewItem).ToArray());

   internal static RecipeOverviewItem MapRecipesOverviewItem(RecipeOverviewItemDto dto)
      => new(
         dto.Id,
         dto.Title,
         dto.Image);

   internal static RecipeDetail MapRecipe(RecipeDetailDto result)
      => new(
         result.Id,
         result.Name,
         result.Allergens,
         MapIngredients(result.Ingredients),
         MapInstructions(result.Instructions),
         result.LastUpdated,
         result.Author,
         result.Image,
         result.Calories,
         result.ReadyInMinutes);

   internal static RecipeIngredient[] MapIngredients(RecipeIngredientDto[] result)
      => result.Select(recipeIngredientDto =>
            new RecipeIngredient(
               recipeIngredientDto.IngredientName,
               recipeIngredientDto.BaseAmount,
               recipeIngredientDto.Measurement,
               recipeIngredientDto.BaseServings))
         .ToArray();

   internal static Instruction[] MapInstructions(InstructionDto[] result)
      => result.Select(instructionDto =>
            new Instruction(
               instructionDto.Text,
               instructionDto.IsNote,
               instructionDto.Index))
         .ToArray();
}