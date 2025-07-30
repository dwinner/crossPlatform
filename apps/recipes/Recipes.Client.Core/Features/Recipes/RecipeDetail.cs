namespace Recipes.Client.Core.Features.Recipes;

public record RecipeDetail(
   string Id,
   string Name,
   string[] Allergens,
   RecipeIngredient[] Ingredients,
   Instruction[] Instructions,
   DateTime LastUpdated,
   string Author,
   string? Image = null,
   int? Calories = null,
   int? ReadyInMinutes = null);