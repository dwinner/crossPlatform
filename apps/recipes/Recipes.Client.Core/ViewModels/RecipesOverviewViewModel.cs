using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Recipes.Client.Core.Features.Favorites;
using Recipes.Client.Core.Features.Recipes;
using Recipes.Client.Core.Messages;
using Recipes.Client.Core.Navigation;

namespace Recipes.Client.Core.ViewModels;

public class RecipesOverviewViewModel : ObservableObject, INavigatedTo, INavigatedFrom
{
   private const int DefaultPageSize = 7;
   private readonly IFavoritesService _favoritesService;
   private readonly INavigationService _navigationService;
   private readonly IRecipeService _recipeService;
   private bool _loadFailed;
   private AsyncRelayCommand _reloadCommand = null!;
   private RecipeListItemViewModel? _selectedRecipe;
   private int _totalNumberOfRecipes;

   public RecipesOverviewViewModel(
      IRecipeService recipeService,
      IFavoritesService favoritesService,
      INavigationService navigationService)
   {
      _recipeService = recipeService;
      _favoritesService = favoritesService;
      _navigationService = navigationService;

      Recipes = new ObservableCollection<RecipeListItemViewModel>();
      TryLoadMoreItemsCommand = new AsyncRelayCommand(TryLoadMoreItems);
      NavigateToSelectedDetailCommand = new AsyncRelayCommand(NavigateToSelectedDetail);

      WeakReferenceMessenger.Default.Register<CultureChangedMessage>(this, (recipient, changedMsg) =>
      {
         Recipes.Clear();
         if (recipient is RecipesOverviewViewModel recipesOverviewVm)
         {
            _ = recipesOverviewVm.LoadRecipes(DefaultPageSize, 0);
         }
      });

      _ = LoadRecipes(DefaultPageSize, 0);
   }

   public ObservableCollection<RecipeListItemViewModel> Recipes { get; }

   public RecipeListItemViewModel? SelectedRecipe
   {
      get => _selectedRecipe;
      set => SetProperty(ref _selectedRecipe, value);
   }

   public int TotalNumberOfRecipes
   {
      get => _totalNumberOfRecipes;
      set => SetProperty(ref _totalNumberOfRecipes, value);
   }

   public bool LoadFailed
   {
      get => _loadFailed;
      set => SetProperty(ref _loadFailed, value);
   }

   public AsyncRelayCommand TryLoadMoreItemsCommand { get; }

   public AsyncRelayCommand NavigateToSelectedDetailCommand { get; }

   public AsyncRelayCommand ReloadCommand
   {
      get => _reloadCommand;
      set => SetProperty(ref _reloadCommand, value);
   }

   public Task OnNavigatedFrom(NavigationType navigationType) => Task.CompletedTask;

   public Task OnNavigatedTo(NavigationType navigationType) => Task.CompletedTask;

   private async Task LoadRecipes(int pageSize, int page)
   {
      LoadFailed = false;

      var recipesResult = await _recipeService.LoadRecipes(pageSize, page)
         .ConfigureAwait(false);
      var favoritesResult = await _favoritesService.LoadFavorites()
         .ConfigureAwait(false);
      if (recipesResult.IsSuccess)
      {
         TotalNumberOfRecipes = recipesResult.Data.TotalItems;
         recipesResult.Data.Recipes.ToList().ForEach(recipe =>
         {
            var isFavorite = favoritesResult?.Contains(recipe.Id) ?? false;
            Recipes.Add(
               new RecipeListItemViewModel(recipe.Id, recipe.Title, isFavorite, recipe.Image));
         });
      }
      else
      {
         LoadFailed = true;
         ReloadCommand = new AsyncRelayCommand(() => LoadRecipes(pageSize, page));
      }
   }

   private async Task NavigateToSelectedDetail()
   {
      if (SelectedRecipe is not null)
      {
         await _navigationService.GoToRecipeDetail(SelectedRecipe.Id)
            .ConfigureAwait(false);
         SelectedRecipe = null;
      }
   }

   private Task TryLoadMoreItems() => LoadRecipes(DefaultPageSize, Recipes.Count / DefaultPageSize);
}