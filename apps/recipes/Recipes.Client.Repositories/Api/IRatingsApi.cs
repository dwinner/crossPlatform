using Recipes.Shared;
using Refit;

namespace Recipes.Client.Repositories.Api;

/*
 * FIXME: Pay attention that Get paths are hardcoded like in WEB API
 */

public interface IRatingsApi
{
   [Get("/recipe/{recipeId}/ratings")]
   Task<ApiResponse<RatingDto[]>> GetRatings(string recipeId);

   [Get("/recipe/{recipeId}/ratingssummary")]
   Task<ApiResponse<RatingsSummaryDto>> GetRatingsSummary(string recipeId);
}