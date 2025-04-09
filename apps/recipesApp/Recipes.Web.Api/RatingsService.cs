using System.Text;
using System.Text.Json;
using Recipes.Shared;

namespace Recipes.Web.Api;

public class RatingsService
{
   private const double DefaultMaxRating = 4;

   private static readonly JsonSerializerOptions SerializeOptions = new()
   {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase
   };

   public RatingsSummaryDto LoadRatingsSummary(string recipeId)
   {
      var ratings = LoadRatings(recipeId);
      return new RatingsSummaryDto(
         ratings.Length,
         DefaultMaxRating,
         ratings.Average(ratingDto => ratingDto.Rating));
   }

   public RatingDto[] LoadRatings(string recipeId)
   {
      var ratings = ReadRatingsFromStream();
      return ratings.Where(ratingDto => ratingDto.RecipeId == recipeId).ToArray();
   }

   private static RatingDto[] ReadRatingsFromStream()
   {
      string json;
      const string ratingsFile = "ratings.json";
      using (var reader = new StreamReader(ratingsFile, Encoding.UTF8))
      {
         json = reader.ReadToEnd();
      }

      return JsonSerializer.Deserialize<RatingDto[]>(json, SerializeOptions) ?? [];
   }
}