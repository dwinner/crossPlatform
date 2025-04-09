namespace Recipes.Shared;

public record RecipeIngredientDto(
   string IngredientName,
   double BaseAmount,
   string Measurement,
   int BaseServings);