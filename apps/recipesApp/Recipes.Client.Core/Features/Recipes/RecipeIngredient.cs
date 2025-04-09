namespace Recipes.Client.Core.Features.Recipes;

public record RecipeIngredient(
   string IngredientName,
   double BaseAmount,
   string Measurement,
   int BaseServings);