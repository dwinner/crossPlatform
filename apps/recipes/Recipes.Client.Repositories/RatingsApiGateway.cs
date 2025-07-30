using Recipes.Client.Core;
using Recipes.Client.Core.Features.Ratings;
using Recipes.Client.Repositories.Api;
using static Recipes.Client.Repositories.Mappers.RatingsMapper;

namespace Recipes.Client.Repositories;

internal class RatingsApiGateway : ApiGateway, IRatingsRepository
{
   private readonly IRatingsApi _api;

   public RatingsApiGateway(IRatingsApi api) => _api = api;

   public Task<Result<IReadOnlyCollection<Rating>>> GetRatings(string recipeId)
      => InvokeAndMap(_api.GetRatings(recipeId), MapRatings);

   public Task<Result<RatingsSummary>> GetRatingsSummary(string recipeId)
      => InvokeAndMap(_api.GetRatingsSummary(recipeId), MapRatingSummary);
}