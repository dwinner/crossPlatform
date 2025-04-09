using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Recipes.Client.Core.Features.Recipes;
using Recipes.Client.Core.Navigation;
using Recipes.Client.Core.Services;
using Recipes.Client.Core.Validation;

namespace Recipes.Client.Core.ViewModels;

public class AddRatingViewModel : ObservableValidator,
   INavigationParameterReceiver,
   INavigatedFrom,
   INavigatable
{
   private const string EmailValidationRegex = @"^[aA-zZ0-9]+@[aA-zZ]+\.[aA-zZ]{2,3}$";
   private const string RangeDecimalRegex = @"^\d+(\.\d{1,1})?$";
   private const int DisplayNameMinLength = 5;
   private const int DisplayNameMaxLength = 25;
   private const double RatingMinVal = 0d;
   private const double RatingMaxVal = 4d;
   private const int ReviewMinLength = 10;
   private const int ReviewMaxLength = 250;
   private readonly IDialogService _dialogService;
   private string _displayName = string.Empty;
   private string _emailAddress = string.Empty;
   private string _ratingInput = string.Empty;
   private string _recipeTitle = string.Empty;
   private string _review = string.Empty;

   public AddRatingViewModel(INavigationService navigationService, IDialogService dialogService)
   {
      _dialogService = dialogService;
      GoBackCommand = new RelayCommand(() => navigationService.GoBack());
      SubmitCommand = new AsyncRelayCommand(OnSubmit, () => !HasErrors);
      Errors = new ObservableCollection<ValidationResult>();
      ErrorExposer = new ValidationErrorExposer(this);
      ErrorsChanged += OnErrorsChanged;
      ResetInputFields();
   }

   public string RecipeTitle
   {
      get => _recipeTitle;
      private set => SetProperty(ref _recipeTitle, value);
   }

   [Required]
   [RegularExpression(EmailValidationRegex)]
   public string EmailAddress
   {
      get => _emailAddress;
      set
      {
         SetProperty(ref _emailAddress, value, true);
         OnPropertyChanged(nameof(EmailValidationErrors));
      }
   }

   [Required]
   [MinLength(DisplayNameMinLength)]
   [MaxLength(DisplayNameMaxLength)]
   public string DisplayName
   {
      get => _displayName;
      set => SetProperty(ref _displayName, value, true);
   }

   [Required]
   [RegularExpression(RangeDecimalRegex)]
   [Range(RatingMinVal, RatingMaxVal)]
   public string RatingInput
   {
      get => _ratingInput;
      set
      {
         SetProperty(ref _ratingInput, value, true);
         ValidateProperty(Review, nameof(Review));
      }
   }

   [CustomValidation(typeof(AddRatingViewModel), nameof(ValidateReview))]
   [EmptyOrWithinRange(MinLength = ReviewMinLength, MaxLength = ReviewMaxLength)]
   public string Review
   {
      get => _review;
      set => SetProperty(ref _review, value, true);
   }

   public RelayCommand GoBackCommand { get; }

   public AsyncRelayCommand SubmitCommand { get; }

   public List<ValidationResult> EmailValidationErrors => GetErrors(nameof(EmailAddress)).ToList();

   public ValidationErrorExposer ErrorExposer { get; }

   public ObservableCollection<ValidationResult> Errors { get; }

   // FIXME: hardcoded non L10n string
   public Task<bool> CanNavigateFrom(NavigationType navigationType) =>
      _dialogService.AskYesNo("Leaving this page...", "Are you sure you want to leave this page?");

   public Task OnNavigatedFrom(NavigationType navigationType)
   {
      if (navigationType == NavigationType.Back)
      {
         ErrorsChanged -= OnErrorsChanged;
         ErrorExposer.Dispose();
         ResetInputFields();
      }

      return Task.CompletedTask;
   }

   // FIXME: Bad idea to base on the hardcoded string as a key
   public Task OnNavigatedTo(Dictionary<string, object> parameters) =>
      parameters["recipe"] is RecipeDetail recipeDetail
         ? LoadData(recipeDetail)
         : Task.CompletedTask;

   public static ValidationResult ValidateReview(string review, ValidationContext context)
   {
      var instance = (AddRatingViewModel)context.ObjectInstance;
      if (double.TryParse(instance.RatingInput, out var rating)
          && rating <= 2 && string.IsNullOrEmpty(review))
      {
         // FIXME: non localized string used
         return new ValidationResult("A review is mandatory when rating the recipe 2 or less.");
      }

      return ValidationResult.Success!;
   }

   private void OnErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
   {
      Errors.Clear();
      GetErrors().ToList().ForEach(Errors.Add);
#if DEBUG
      Errors.ToList().ForEach(validationResult => Console.WriteLine(validationResult.ErrorMessage));
#endif
      SubmitCommand.NotifyCanExecuteChanged();
   }

   private void ResetInputFields()
   {
      EmailAddress = string.Empty;
      DisplayName = string.Empty;
      RatingInput = string.Empty;
      Review = string.Empty;
   }

   private async Task OnSubmit()
   {
      // FIXME: non localized strings are used
      var result = await _dialogService.AskYesNo(
         "Are you sure?",
         "Are you sure you want to add this rating?");
      if (result)
      {
         // APPLY: Submit data
         await _dialogService.Notify(
            "Rating sent",
            "Thank you for your feedback!");

         GoBackCommand.Execute(null);
      }
   }

   private Task LoadData(RecipeDetail recipe)
   {
      RecipeTitle = recipe.Name;
      return Task.CompletedTask;
   }
}