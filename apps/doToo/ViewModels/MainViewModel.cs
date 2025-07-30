using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoToo.Models;
using DoToo.Repositories;
using DoToo.Views;

// ReSharper disable AsyncVoidLambda
#pragma warning disable CA1416

namespace DoToo.ViewModels;

public partial class MainViewModel : ViewModel
{
   private readonly ITodoItemRepository _repository;
   private readonly IServiceProvider _services;

   [ObservableProperty] private ObservableCollection<TodoItemViewModel> _items = [];

   [ObservableProperty] private TodoItemViewModel? _selectedItem;

   [ObservableProperty] private bool _showAll;

   public MainViewModel(ITodoItemRepository repository, IServiceProvider services)
   {
      _repository = repository;
      repository.OnItemAdded += (_, item) => Items.Add(CreateTodoItemViewModel(item));
      repository.OnItemUpdated += (_, _) => Task.Run(async () =>
      {
         await LoadDataAsync().ConfigureAwait(true);
         Debug.WriteLine("Loaded");
      });
      _services = services;
      Task.Run(async () =>
      {
         await LoadDataAsync().ConfigureAwait(true);
         Debug.WriteLine("Loaded");
      });
   }

   [RelayCommand]
   private async Task ToggleFilterAsync()
   {
      ShowAll = !ShowAll;
      await LoadDataAsync().ConfigureAwait(true);
      Debug.WriteLine(nameof(ToggleFilterAsync));
   }

   private async Task LoadDataAsync()
   {
      var items = await _repository.GetItemsAsync().ConfigureAwait(true);
      if (!ShowAll)
      {
         items = items.Where(x => x.Completed == false).ToList();
      }

      var itemVms = items.Select(CreateTodoItemViewModel);
      Items = new ObservableCollection<TodoItemViewModel>(itemVms);
   }

   private TodoItemViewModel CreateTodoItemViewModel(TodoItem item)
   {
      var itemViewModel = new TodoItemViewModel(item);
      itemViewModel.ItemStatusChanged += ItemStatusChanged;
      return itemViewModel;
   }

   private void ItemStatusChanged(object? sender, EventArgs e)
   {
      if (sender is not TodoItemViewModel item)
      {
         return;
      }

      if (!ShowAll && item.Item.Completed)
      {
         Items.Remove(item);
      }

      Task.Run(async () =>
      {
         await _repository.UpdateItemAsync(item.Item).ConfigureAwait(true);
         Debug.WriteLine(nameof(ItemStatusChanged));
      });
   }

   [RelayCommand]
   private async Task AddItemAsync()
   {
      Debug.Assert(Navigation != null, $"{nameof(Navigation)} != null");
      await Navigation.PushAsync(_services.GetRequiredService<ItemView>())
         .ConfigureAwait(true);
      Debug.WriteLine(nameof(AddItemAsync));
   }

   partial void OnSelectedItemChanging(TodoItemViewModel? value) =>
      MainThread.BeginInvokeOnMainThread(async () =>
      {
         if (value != null)
         {
            await NavigateToItemAsync(value).ConfigureAwait(true);
         }

         Debug.WriteLine(nameof(OnSelectedItemChanging));
      });

   private async Task NavigateToItemAsync(TodoItemViewModel item)
   {
      var itemView = _services.GetRequiredService<ItemView>();
      var itemViewModel = itemView.BindingContext as ItemViewModel;
      Debug.Assert(itemViewModel != null, $"{nameof(itemViewModel)} != null");
      itemViewModel.Item = item.Item;
      itemView.Title = "Edit todo item";
      Debug.Assert(Navigation != null, $"{nameof(Navigation)} != null");
      await Navigation.PushAsync(itemView)
         .ConfigureAwait(true);
      Debug.WriteLine(nameof(NavigateToItemAsync));
   }
}