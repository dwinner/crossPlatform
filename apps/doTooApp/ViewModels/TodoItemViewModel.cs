using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoToo.Models;

namespace DoToo.ViewModels;

public partial class TodoItemViewModel(TodoItem item) : ViewModel
{
   [ObservableProperty] private TodoItem _item = item;

   public string StatusText => Item.Completed ? "Reactivate" : "Completed";

   public event EventHandler? ItemStatusChanged;

   [RelayCommand]
   private void ToggleCompleted()
   {
      Item.Completed = !Item.Completed;
      ItemStatusChanged?.Invoke(this, EventArgs.Empty);
   }
}