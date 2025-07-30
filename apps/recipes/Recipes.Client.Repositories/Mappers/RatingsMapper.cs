using Recipes.Client.Core.Features.Ratings;
using Recipes.Shared;

namespace Recipes.Client.Repositories.Mappers;

/*
 * TOREFACTOR: Such mapping can be done via overloaded cast operators or Automapper lib.
 */

internal static class RatingsMapper
{
   internal static IReadOnlyCollection<Rating> MapRatings(RatingDto[] ratingDtoArray)
      => ratingDtoArray.Select(ratingDto =>
            new Rating(
               ratingDto.Id,
               ratingDto.RecipeId,
               ratingDto.Rating,
               ratingDto.UserName,
               ratingDto.Review))
         .ToArray();

   internal static RatingsSummary MapRatingSummary(RatingsSummaryDto dto) =>
      new(
         dto.TotalReviews,
         dto.MaxRating,
         dto.AverageRating);
}