using CommunityToolkit.Mvvm.ComponentModel;

namespace Recipes.Client.Core.ViewModels;

public class RecipeRatingsSummaryViewModel : ObservableObject
{
   private const int DefaultTotalReviews = 15;
   private const double DefaultMaxRating = 4d;
   private const double DefaultAverageRating = 3.6d;

   public RecipeRatingsSummaryViewModel(
      int totalReviews = DefaultTotalReviews,
      double? averageRating = DefaultAverageRating,
      double maxRating = DefaultMaxRating)
   {
      TotalReviews = totalReviews;
      AverageRating = averageRating;
      MaxRating = maxRating;
   }

   public int TotalReviews { get; }

   public double MaxRating { get; }

   public double? AverageRating { get; }
}