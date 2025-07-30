using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Recipes.Client.Core.Features.Ratings;
using Recipes.Client.Core.Features.Recipes;
using Recipes.Client.Core.Navigation;
using Recipes.Client.Core.Services;

namespace Recipes.Client.Core.ViewModels;

public class RecipeRatingsDetailViewModel : ObservableObject, INavigationParameterReceiver, INavigatedTo, INavigatedFrom
{
   private readonly IDialogService _dialogService;
   private readonly INavigationService _navigationService;
   private readonly IRatingsService _ratingsService;
   private List<RatingGroup> _groupedReviews = new();
   private string _recipeTitle = string.Empty;

   public RecipeRatingsDetailViewModel(
      INavigationService navigationService,
      IRatingsService ratingsService,
      IDialogService dialogService)
   {
      _ratingsService = ratingsService;
      _dialogService = dialogService;
      _navigationService = navigationService;

      ReportReviewsCommand = new RelayCommand(ReportReviews, () => SelectedReviews.Any());
      GoBackCommand = new RelayCommand(() => navigationService.GoBack());

      SelectedReviews.CollectionChanged += OnSelectedReviewsChanged;
   }

   public string RecipeTitle
   {
      get => _recipeTitle;
      set => SetProperty(ref _recipeTitle, value);
   }

   public List<RatingGroup> GroupedReviews
   {
      get => _groupedReviews;
      private set => SetProperty(ref _groupedReviews, value);
   }

   public ObservableCollection<object> SelectedReviews { get; } = [];

   public RelayCommand ReportReviewsCommand { get; }

   public RelayCommand GoBackCommand { get; }

   public Task OnNavigatedFrom(NavigationType navigationType) => Task.CompletedTask;

   public Task OnNavigatedTo(NavigationType navigationType) => Task.CompletedTask;

   public Task OnNavigatedTo(Dictionary<string, object> parameters)
   {
      // FIXME: bad idea to have a string as a map key there
      if (parameters["recipe"] is RecipeDetail recipeDetail)
      {
         return LoadData(recipeDetail);
      }

      return Task.CompletedTask;
   }

   private async Task LoadData(RecipeDetail recipeDetail)
   {
      RecipeTitle = recipeDetail.Name;
      var loadRatings = await _ratingsService.LoadRatings(recipeDetail.Id)
         .ConfigureAwait(false);
      if (loadRatings is { IsSuccess: true, Data: var ratings })
      {
         GroupedReviews = ratings
            .Select(rating => new UserReviewViewModel(rating.UserName, rating.Score, rating.Review))
            .GroupBy(reviewVm => Math.Round(reviewVm.Rating / .5) * .5)
            .OrderByDescending(reviewVm => reviewVm.Key)
            .Select(grp => new RatingGroup(grp.Key.ToString(CultureInfo.CurrentCulture), grp.ToList()))
            .ToList();
      }
      else
      {
         // FIXME: unlocalized string there
         var shouldRetry = await _dialogService.AskYesNo("Failed to load", "Retry?")
            .ConfigureAwait(false);
         if (shouldRetry)
         {
            await LoadData(recipeDetail).ConfigureAwait(false);
         }
         else
         {
            await _navigationService.GoBack().ConfigureAwait(false);
         }
      }
   }

   private void OnSelectedReviewsChanged(object? sender, NotifyCollectionChangedEventArgs e)
      => ReportReviewsCommand.NotifyCanExecuteChanged();

   private void ReportReviews()
   {
      // var selectedReviews = SelectedReviews.Cast<UserReviewViewModel>().ToList();
      // APPLY: do reporting
      SelectedReviews.Clear();
   }
}