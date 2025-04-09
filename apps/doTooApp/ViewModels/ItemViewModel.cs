using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoToo.Models;
using DoToo.Repositories;

#pragma warning disable CA1416

namespace DoToo.ViewModels;

public partial class ItemViewModel(ITodoItemRepository repository) : ViewModel
{
   [ObservableProperty] private TodoItem _item = new()
   {
      Due = DateTime.Now.AddDays(1)
   };

   [RelayCommand]
   private async Task SaveAsync()
   {
      await repository.AddOrUpdateAsync(Item).ConfigureAwait(true);
      if (Navigation != null)
      {
         await Navigation.PopAsync().ConfigureAwait(true);
      }
   }
}