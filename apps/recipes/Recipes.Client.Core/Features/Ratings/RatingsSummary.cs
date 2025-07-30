namespace Recipes.Client.Core.Features.Ratings;

public record RatingsSummary(int TotalReviews, double MaxRating, double? AverageRating);