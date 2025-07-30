namespace Recipes.Client.Core.Features.Ratings;

public record Rating(string Id, string RecipeId, double Score, string UserName, string Review);