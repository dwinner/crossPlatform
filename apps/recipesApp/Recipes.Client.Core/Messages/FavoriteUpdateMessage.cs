namespace Recipes.Client.Core.Messages;

public class FavoriteUpdateMessage
{
   public FavoriteUpdateMessage(string recipeId, bool isFavorite)
   {
      RecipeId = recipeId;
      IsFavorite = isFavorite;
   }

   public string RecipeId { get; }

   public bool IsFavorite { get; }
}