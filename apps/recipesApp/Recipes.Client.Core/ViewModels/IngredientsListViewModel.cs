using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Recipes.Client.Core.Features.Recipes;
using Recipes.Client.Core.Messages;

namespace Recipes.Client.Core.ViewModels;

public class IngredientsListViewModel : ObservableObject
{
   private readonly WeakReferenceMessenger _messenger;
   private int _numberOfServings = 4;

   public IngredientsListViewModel(IReadOnlyList<RecipeIngredient> ingredients)
   {
      // TONOTE: Here we can safely use auto mapper to transform RecipeIngredient to RecipeIngredientViewModel
      Ingredients = ingredients.Select(ingredient =>
            new RecipeIngredientViewModel(
               ingredient.IngredientName,
               ingredient.BaseAmount,
               ingredient.Measurement,
               ingredient.BaseServings))
         .ToList();
      _messenger = WeakReferenceMessenger.Default;
   }

   public int NumberOfServings
   {
      get => _numberOfServings;
      set
      {
         if (SetProperty(ref _numberOfServings, value))
         {
            _messenger.Send(new ServingsChangedMessage(value));
         }
      }
   }

   public List<RecipeIngredientViewModel> Ingredients { get; }
}