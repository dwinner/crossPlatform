using c4_LocalDatabaseConnection.DataAccess;
using c4_LocalDatabaseConnection.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c4_LocalDatabaseConnection.ViewModels;

public partial class CustomerDetailViewModel : ObservableObject, IQueryAttributable
{
   [ObservableProperty] private Customer _item;

   protected Func<Customer, Task> ParentRefreshAction { get; private set; }

   public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
   {
      if (query.TryGetValue(nameof(Item), out var currentItem))
      {
         Item = (Customer)currentItem;
      }

      if (query.TryGetValue(nameof(ParentRefreshAction), out var parentRefreshAction))
      {
         ParentRefreshAction = (Func<Customer, Task>)parentRefreshAction;
      }

      query.Clear();
   }

   [RelayCommand]
   private async Task ShowEditFormAsync()
   {
      using var uof = new CrmUnitOfWork();
      var editedItem = await uof.Items.GetByIdAsync(Item.Id);
      await Shell.Current.GoToAsync(nameof(CustomerEditPage),
         new Dictionary<string, object>
         {
            { nameof(ParentRefreshAction), (Func<Customer, Task>)ItemEditedAsync },
            { nameof(Item), editedItem }
         });
   }

   private async Task ItemEditedAsync(Customer customer)
   {
      Item = customer;
      await ParentRefreshAction(customer);
   }
}