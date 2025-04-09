using Recipes.Client.Core.Features.Recipes;
using Recipes.Client.Core.Navigation;

namespace Recipes.Mobile.Navigation;

public class NavigationService : INavigationService, INavigationInterceptor
{
   private WeakReference<INavigatedFrom>? _previousFrom;

   public async Task OnNavigatedTo(object bindingContext, NavigationType navigationType)
   {
      if (_previousFrom is not null
          && _previousFrom.TryGetTarget(out var from))
      {
         await from.OnNavigatedFrom(navigationType);
      }

      if (bindingContext is INavigatedTo to)
      {
         await to.OnNavigatedTo(navigationType);
      }

      if (bindingContext is INavigatedFrom navigatedFrom)
      {
         _previousFrom = new WeakReference<INavigatedFrom>(navigatedFrom);
      }
      else
      {
         _previousFrom = null;
      }
   }

   public Task<bool> CanNavigate(object bindingContext, NavigationType type)
   {
      if (bindingContext is INavigatable navigatable)
      {
         return navigatable.CanNavigateFrom(type);
      }

      return Task.FromResult(true);
   }

   public Task GoToRecipeDetail(string recipeId)
      => Navigate("RecipeDetail",
         new Dictionary<string, object> { { "id", recipeId } });

   public Task GoToRecipeRatingDetail(RecipeDetail recipe)
      => Navigate("RecipeRating",
         new Dictionary<string, object> { { "recipe", recipe } });

   public Task GoToChooseLanguage(string currentLanguage)
      => Navigate("PickLanguagePage",
         new Dictionary<string, object> { { "language", currentLanguage } });

   public Task GoToAddRating(RecipeDetail recipe)
      => Navigate("AddRating",
         new Dictionary<string, object> { { "recipe", recipe } });

   public Task GoBack()
      => Shell.Current.GoToAsync("..");

   public async Task GoBackAndReturn(Dictionary<string, object> parameters)
   {
      await GoBack();
      if (Shell.Current.CurrentPage.BindingContext is INavigationParameterReceiver receiver)
      {
         await receiver.OnNavigatedTo(parameters);
      }
   }

   public Task GoToOverview() => throw new NotImplementedException();

   private async Task Navigate(string pageName, Dictionary<string, object> parameters)
   {
      await Shell.Current.GoToAsync(pageName);
      if (Shell.Current.CurrentPage.BindingContext is INavigationParameterReceiver receiver)
      {
         await receiver.OnNavigatedTo(parameters);
      }
   }
}