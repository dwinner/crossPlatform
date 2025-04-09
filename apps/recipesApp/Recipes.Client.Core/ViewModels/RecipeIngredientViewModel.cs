using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Recipes.Client.Core.Messages;

namespace Recipes.Client.Core.ViewModels;

public class RecipeIngredientViewModel : ObservableObject
{
   private const int DefaultBaseServings = 4;
   private readonly double _baseAmount;
   private readonly int _baseServings;
   private double? _displayAmount;

   public RecipeIngredientViewModel(
      string ingredientName, double baseAmount, string? measurement = null, int baseServings = DefaultBaseServings)
   {
      IngredientName = ingredientName;
      Measurement = measurement;
      _baseAmount = baseAmount;
      _baseServings = baseServings;

      WeakReferenceMessenger.Default.Register<ServingsChangedMessage>(
         this,
         (obj, msg) =>
            ((RecipeIngredientViewModel)obj).UpdateServings(msg.Value));
   }

   public string IngredientName { get; }

   public string? Measurement { get; }

   public double DisplayAmount
   {
      get => _displayAmount ?? _baseAmount;
      set => SetProperty(ref _displayAmount, value);
   }

   private void UpdateServings(int servings)
   {
      var factor = servings / (double)_baseServings;
      DisplayAmount = factor * _baseAmount;
   }
}