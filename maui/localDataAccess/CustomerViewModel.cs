using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace LocalDataAccess;

public class CustomerViewModel : ObservableObject
{
   private readonly AsyncRelayCommand _addNewCommand;
   private readonly DatabaseConnection _dataAccess;
   private readonly AsyncRelayCommand _deleteCommand;
   private readonly AsyncRelayCommand _saveAllCommand;
   private ObservableCollection<Customer> _customers;
   private Customer _selectedCustomer;

   public CustomerViewModel()
   {
      _dataAccess = new DatabaseConnection();
      Customers = new ObservableCollection<Customer>();
      _saveAllCommand = new AsyncRelayCommand(SaveAllAsync);
      _addNewCommand = new AsyncRelayCommand(AddNewAsync);
      _deleteCommand = new AsyncRelayCommand(DeleteAsync);

      Task.Run(async () =>
      {
         var customers = await _dataAccess.GetCustomersAsync().ConfigureAwait(true);
         if (customers.Count > 0)
         {
            Customers = new ObservableCollection<Customer>(customers);
         }
         else
         {
            await AddCustomerAsync().ConfigureAwait(true);
         }
      });
   }

   public Customer SelectedCustomer
   {
      get => _selectedCustomer;
      set => SetProperty(ref _selectedCustomer, value);
   }

   public ObservableCollection<Customer> Customers
   {
      get => _customers;
      set => SetProperty(ref _customers, value);
   }

   public IAsyncRelayCommand SaveAllCommand => _saveAllCommand;

   public IAsyncRelayCommand AddNewCommand => _addNewCommand;

   public IAsyncRelayCommand DeleteCommand => _deleteCommand;

   private async Task DeleteAsync()
   {
      if (SelectedCustomer == null)
      {
         return;
      }

      Customers.Remove(SelectedCustomer);
      await _dataAccess.DeleteCustomerAsync(SelectedCustomer).ConfigureAwait(true);
   }

   private async Task AddNewAsync()
   {
      await AddCustomerAsync().ConfigureAwait(true);
      Debug.WriteLine(nameof(AddNewAsync));
   }

   private async Task SaveAllAsync()
   {
      await _dataAccess.SaveAllCustomersAsync(Customers)
         .ConfigureAwait(true);
      Debug.WriteLine(nameof(SaveAllAsync));
   }

   private async Task AddCustomerAsync()
   {
      var newCustomer = new Customer
      {
         CompanyName = "Company name...",
         PhysicalAddress = "Address...",
         Country = "Country..."
      };

      Customers.Add(newCustomer);
      await _dataAccess.SaveCustomerAsync(newCustomer).ConfigureAwait(true);
      Debug.WriteLine(nameof(AddCustomerAsync));
   }
}