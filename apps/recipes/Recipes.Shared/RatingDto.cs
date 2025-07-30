namespace Recipes.Shared;

public record RatingDto(string Id, string RecipeId, double Rating, string UserName, string Review);