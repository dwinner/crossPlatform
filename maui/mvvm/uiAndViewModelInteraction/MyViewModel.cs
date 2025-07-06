using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace c2_DecoupleViewAndViewModel;

public partial class MyViewModel(IMessenger messenger) : ObservableObject
{
   [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AddCustomerCommand))]
   private ObservableCollection<Customer>? _customers;

   [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(InitializeCommand))]
   private bool _isInitialized;

   [RelayCommand(CanExecute = nameof(CanInitialize))]
   private async Task InitializeAsync()
   {
      Customers = new ObservableCollection<Customer>(await DummyService.GetCustomersAsync());
      IsInitialized = true;
   }

   [RelayCommand(CanExecute = nameof(CanAddCustomer))]
   private void AddCustomer()
   {
      if (Customers == null)
      {
         return;
      }

      Customers.Add(new Customer
      {
         Id = Customers.Count,
         Name = "New Customer"
      });

      var addedCustomer = Customers[^1];
      messenger.Send(addedCustomer);
   }

   private bool CanAddCustomer() => Customers != null;

   private bool CanInitialize() => !IsInitialized;
}

public static class DummyService
{
   public static async Task<IEnumerable<Customer>> GetCustomersAsync()
   {
      await Task.Delay(5000);
      var customers = new List<Customer>();
      for (var i = 0; i < 40; i++)
      {
         customers.Add(new Customer { Id = i, Name = $"Customer{i}" });
      }

      return customers;
   }
}

public class Customer
{
   public int Id { get; set; }

   public string? Name { get; set; }
}