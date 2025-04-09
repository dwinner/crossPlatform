namespace Recipes.Client.Core.ViewModels;

public class UserReviewViewModel
{
   public UserReviewViewModel(string username, double rating, string? review = null)
   {
      UserName = username;
      Rating = rating;
      Review = review;
   }

   public double Rating { get; }

   public string UserName { get; }

   public string? Review { get; }
}