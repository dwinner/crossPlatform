using System.Collections.ObjectModel;
using c4_LocalDatabaseConnection.DataAccess;
using c4_LocalDatabaseConnection.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c4_LocalDatabaseConnection.ViewModels;

public partial class MainViewModel : ObservableObject
{
   [ObservableProperty] private ObservableCollection<Customer> _customers;

   [ObservableProperty] private bool _refreshing;

   [RelayCommand]
   private void Showing()
   {
      Refreshing = true;
   }

   [RelayCommand]
   private async Task LoadCustomersAsync()
   {
      await Task.Run(() =>
      {
         using CrmContext context = new CrmContext();
         Customers = new ObservableCollection<Customer>(context.Customers);
      });
      Refreshing = false;
   }

   [RelayCommand]
   private async Task DeleteCustomer(Customer customer)
   {
      await using var context = new CrmContext();
      context.Customers.Remove(customer);
      await context.SaveChangesAsync();
      Customers.Remove(customer);
   }

   [RelayCommand]
   private async Task ShowNewFormAsync()
   {
      await Shell.Current.GoToAsync(nameof(CustomerEditPage),
         new Dictionary<string, object>
         {
            { "ParentRefreshAction", (Func<Customer, Task>)RefreshAddedAsync },
            { "Item", new Customer() }
         });
   }

   private Task RefreshAddedAsync(Customer addedCustomer)
   {
      Customers.Add(addedCustomer);
      return Task.CompletedTask;
   }
}