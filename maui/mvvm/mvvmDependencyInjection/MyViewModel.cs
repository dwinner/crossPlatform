#nullable enable

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c2_DecoupleViewAndViewModel;

public partial class MyViewModel(IDummyService dataService) : ObservableObject
{
   [ObservableProperty] private ObservableCollection<Customer>? _customers;

   [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(InitializeCommand))]
   private bool _isInitialized;

   [RelayCommand(CanExecute = nameof(CanInitialize))]
   private async Task InitializeAsync()
   {
      Customers = new ObservableCollection<Customer>(await dataService.GetCustomersAsync());
      IsInitialized = true;
   }

   private bool CanInitialize() => !IsInitialized;
}

public class Customer
{
   public int Id { get; set; }

   public string? Name { get; set; }
}

public interface IDummyService
{
   Task<IEnumerable<Customer>> GetCustomersAsync();
}

public class DummyService : IDummyService
{
   public async Task<IEnumerable<Customer>> GetCustomersAsync()
   {
      await Task.Delay(5000);
      return new List<Customer>
      {
         new() { Id = 1, Name = "Jim" },
         new() { Id = 2, Name = "Bob" }
      };
   }
}