using System;
using System.Reactive.Linq;
using JetBrains.Annotations;
using ReactiveUI;
using ToDoList.DataModel;
using ToDoList.Services;

namespace ToDoList.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
   // NOTE: this has a dependency on the ToDoListService
   private readonly ToDoListService _service = new();
   private ViewModelBase _contentViewModel;

   public MainWindowViewModel()
   {
      ToDoList = new ToDoListViewModel(_service.GetItems());
      _contentViewModel = ToDoList;
   }

   public ToDoListViewModel ToDoList { get; }

   public ViewModelBase ContentViewModel
   {
      get => _contentViewModel;
      private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
   }

   [UsedImplicitly]
   public void AddItem()
   {
      AddItemViewModel addItemViewModel = new();

      addItemViewModel.OkCommand
         .Merge(addItemViewModel.CancelCommand.Select(_ => (ToDoItem?)null))
         .Take(1).Subscribe(item =>
         {
            if (item != null)
            {
               ToDoList.ListItems.Add(item);
            }

            ContentViewModel = ToDoList;
         });

      ContentViewModel = addItemViewModel;
   }
}