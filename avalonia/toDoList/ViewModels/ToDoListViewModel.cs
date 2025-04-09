using System.Collections.Generic;
using System.Collections.ObjectModel;
using ToDoList.DataModel;

namespace ToDoList.ViewModels;

public class ToDoListViewModel : ViewModelBase
{
   public ToDoListViewModel(IEnumerable<ToDoItem> items) => ListItems = new ObservableCollection<ToDoItem>(items);

   public ObservableCollection<ToDoItem> ListItems { get; }
}