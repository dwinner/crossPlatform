using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Recipes.Client.Core.Features.Favorites;
using Recipes.Client.Core.Features.Ratings;
using Recipes.Client.Core.Features.Recipes;
using Recipes.Client.Core.Navigation;
using Recipes.Client.Core.Services;

namespace Recipes.Client.Core.ViewModels;

public class RecipeDetailViewModel :
   ObservableObject,
   INavigationParameterReceiver,
   INavigatedTo,
   INavigatedFrom
{
   private const int MaxUpdatedAllowed = 5;
   private readonly IDialogService _dialogService;
   private readonly IFavoritesService _favoritesService;
   private readonly INavigationService _navigationService;
   private readonly IRatingsService _ratingsService;
   private readonly IRecipeService _recipeService;

   private string[] _allergens = [];
   private string _author = string.Empty;
   private int? _calories;
   private bool _hideAllergenInformation = true;
   private string _image = string.Empty;
   private IngredientsListViewModel _ingredientsList = null!;
   private List<InstructionBaseViewModel> _instructions = null!;
   private bool _isFavorite;
   private bool _isLoading = true;
   private DateTime _lastUpdated;
   private RecipeRatingsSummaryViewModel _ratingSummary = null!;
   private int? _readyInMinutes;
   private RecipeDetail _recipeDto = null!;
   private string _title = string.Empty;
   private int _updateCount;

   public RecipeDetailViewModel(IRecipeService recipeService,
      IFavoritesService favoritesService,
      IRatingsService ratingsService,
      INavigationService navigationService,
      IDialogService dialogService)
   {
      _recipeService = recipeService;
      _favoritesService = favoritesService;
      _ratingsService = ratingsService;
      _navigationService = navigationService;
      _dialogService = dialogService;

      AddAsFavoriteCommand = new AsyncRelayCommand(AddAsFavorite, CanAddAsFavorite);
      RemoveAsFavoriteCommand = new AsyncRelayCommand(RemoveAsFavorite, CanRemoveAsFavorite);
      FavoriteToggledCommand = new AsyncRelayCommand<bool>(FavoriteToggled, _ => _updateCount < MaxUpdatedAllowed);
      UserIsBrowsingCommand = new RelayCommand(UserIsBrowsing);
      AddToShoppingListCommand = new RelayCommand<RecipeIngredientViewModel>(AddToShoppingList);
      RemoveFromShoppingListCommand = new RelayCommand<RecipeIngredientViewModel>(RemoveFromShoppingList);
      NavigateToRatingsCommand = new AsyncRelayCommand(NavigateToRatings);
      NavigateToAddRatingCommand = new AsyncRelayCommand(NavigateToAddRating);
   }

   public string Title
   {
      get => _title;
      set => SetProperty(ref _title, value);
   }

   public string[] Allergens
   {
      get => _allergens;
      set => SetProperty(ref _allergens, value);
   }

   public int? Calories
   {
      get => _calories;
      set => SetProperty(ref _calories, value);
   }

   public int? ReadyInMinutes
   {
      get => _readyInMinutes;
      set => SetProperty(ref _readyInMinutes, value);
   }

   public DateTime LastUpdated
   {
      get => _lastUpdated;
      set => SetProperty(ref _lastUpdated, value);
   }

   public string Author
   {
      get => _author;
      set => SetProperty(ref _author, value);
   }

   public string Image
   {
      get => _image;
      set => SetProperty(ref _image, value);
   }

   public RecipeRatingsSummaryViewModel RatingSummary
   {
      get => _ratingSummary;
      set => SetProperty(ref _ratingSummary, value);
   }

   public List<InstructionBaseViewModel> Instructions
   {
      get => _instructions;
      set => SetProperty(ref _instructions, value);
   }

   public IngredientsListViewModel IngredientsList
   {
      get => _ingredientsList;
      set => SetProperty(ref _ingredientsList, value);
   }

   public bool HideAllergenInformation
   {
      get => _hideAllergenInformation;
      set => SetProperty(ref _hideAllergenInformation, value);
   }

   public bool IsFavorite
   {
      get => _isFavorite;
      set
      {
         if (SetProperty(ref _isFavorite, value))
         {
            AddAsFavoriteCommand.NotifyCanExecuteChanged();
            RemoveAsFavoriteCommand.NotifyCanExecuteChanged();
         }
      }
   }

   public bool IsLoading
   {
      get => _isLoading;
      set => SetProperty(ref _isLoading, value);
   }

   public ObservableCollection<RecipeIngredientViewModel> ShoppingList { get; } = new();
   public IRelayCommand AddAsFavoriteCommand { get; }
   public IRelayCommand RemoveAsFavoriteCommand { get; }
   public IRelayCommand FavoriteToggledCommand { get; }
   public IRelayCommand AddToShoppingListCommand { get; }
   public IRelayCommand RemoveFromShoppingListCommand { get; }
   public IRelayCommand UserIsBrowsingCommand { get; }
   public IAsyncRelayCommand NavigateToRatingsCommand { get; }
   public IAsyncRelayCommand NavigateToAddRatingCommand { get; }

   public Task OnNavigatedFrom(NavigationType navigationType) => Task.CompletedTask;

   public Task OnNavigatedTo(NavigationType navigationType) => Task.CompletedTask;

   public Task OnNavigatedTo(Dictionary<string, object> parameters)
   {
      // FIXME: bad idea to store a simple string as a map key
      var id = parameters["id"];
      return LoadRecipe(id.ToString() ?? throw new InvalidOperationException("Invalid recipe id"));
   }

   private async Task LoadRecipe(string recipeId)
   {
      try
      {
         IsLoading = true;

         var loadRecipeTask = _recipeService.LoadRecipe(recipeId);
         var loadIsFavoriteTask = _favoritesService.IsFavorite(recipeId);
         var loadRatingsTask = _ratingsService.LoadRatingsSummary(recipeId);

         await Task.WhenAll(loadRecipeTask, loadIsFavoriteTask, loadRatingsTask);
         if (!loadRecipeTask.Result.IsSuccess || !loadRatingsTask.Result.IsSuccess)
         {
            // FIXME: non localized message there
            var result = await _dialogService.AskYesNo("Unable to load recipe", "Want to retry?");
            if (result)
            {
               await LoadRecipe(recipeId);
            }
            else
            {
               await _navigationService.GoBack();
            }
         }
         else
         {
            MapRecipeData(loadRecipeTask.Result.Data, loadRatingsTask.Result.Data, loadIsFavoriteTask.Result);
         }
      }
      finally
      {
         IsLoading = false;
      }
   }

   private void MapRecipeData(RecipeDetail recipe, RatingsSummary ratings, bool isFavorite)
   {
      _recipeDto = recipe;
      Title = recipe.Name;
      Allergens = recipe.Allergens;
      Calories = recipe.Calories;
      ReadyInMinutes = recipe.ReadyInMinutes;
      LastUpdated = recipe.LastUpdated;
      Author = recipe.Author;

      // TOREFACTOR: bad idea in using fallback value not on the XAML site
      Image = recipe.Image ?? "fallback.png";
      var instructions = new List<InstructionBaseViewModel>();
      foreach (var item in recipe.Instructions)
      {
         if (item.IsNote)
         {
            instructions.Add(new NoteViewModel(item.Text));
         }
         else
         {
            instructions.Add(new InstructionViewModel(item.Index ?? 0, item.Text));
         }
      }

      Instructions = instructions;
      IngredientsList = new IngredientsListViewModel(recipe.Ingredients);
      IsFavorite = isFavorite;
      RatingSummary = new RecipeRatingsSummaryViewModel(ratings.TotalReviews, ratings.AverageRating, ratings.MaxRating);
   }

   private bool CanAddAsFavorite() => !IsFavorite;

   private Task AddAsFavorite() => UpdateIsFavorite(true);

   private Task RemoveAsFavorite() => UpdateIsFavorite(false);

   private Task UpdateIsFavorite(bool newValue)
   {
      IsFavorite = newValue;
      return FavoriteToggled(newValue);
   }

   private async Task FavoriteToggled(bool isFavorite)
   {
      var recipeId = _recipeDto.Id;
      if (isFavorite)
      {
         await _favoritesService.Add(recipeId);
      }
      else
      {
         await _favoritesService.Remove(recipeId);
      }

      _updateCount++;
      FavoriteToggledCommand.NotifyCanExecuteChanged();
   }

   private bool CanRemoveAsFavorite() => IsFavorite;

   private void UserIsBrowsing()
   {
      // APPLY: Do Logging
   }

   private void AddToShoppingList(RecipeIngredientViewModel? viewModel)
   {
      if (viewModel == null || ShoppingList.Contains(viewModel))
      {
         return;
      }

      ShoppingList.Add(viewModel);
   }

   private void RemoveFromShoppingList(RecipeIngredientViewModel? viewModel)
   {
      if (viewModel != null && ShoppingList.Contains(viewModel))
      {
         ShoppingList.Remove(viewModel);
      }
   }

   private Task NavigateToRatings() => _navigationService.GoToRecipeRatingDetail(_recipeDto);

   private Task NavigateToAddRating() => _navigationService.GoToAddRating(_recipeDto);
}