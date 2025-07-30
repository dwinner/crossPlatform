using System.Web;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using News.Models;
using News.Services;

namespace News.ViewModels;

public partial class HeadlinesViewModel(INewsService newsService, INavigate navigation)
   : ViewModelBase(navigation)
{
   [ObservableProperty] private NewsResult _currentNews = new()
   {
      TotalResults = 0,
      Status = string.Empty,
      Articles = []
   };

   public async Task Initialize(string scope) =>
      await Initialize(scope.ToLower() switch
      {
         "local" => NewsScope.Local,
         "global" => NewsScope.Global,
         "headlines" => NewsScope.Headlines,
         _ => NewsScope.Headlines
      });

   public async Task Initialize(NewsScope scope)
   {
      CurrentNews = await newsService.GetNews(scope);
   }

   [RelayCommand]
   public async Task ItemSelected(Article? article)
   {
      var url = HttpUtility.UrlEncode(article?.Url);
      await Navigation.NavigateTo($"articleview?url={url}");
   }
}