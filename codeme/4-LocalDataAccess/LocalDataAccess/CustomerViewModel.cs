using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LocalDataAccess
{
    public class CustomerViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Customer> _customers;
        public ObservableCollection<Customer> Customers
        {
            get
            {
                return _customers;
            }
            set
            {
                _customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get
            {
                return _selectedCustomer;
            }
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }


        private DatabaseConnection dataAccess;

        public CustomerViewModel()
        {
            dataAccess = new DatabaseConnection();
            Customers = new ObservableCollection<Customer>();

            Task.Run(async () =>
            {
                var customers = await dataAccess.
                    GetCustomersAsync();
                if (customers.Any())
                {
                    Customers =
                      new ObservableCollection<Customer>(customers);
                }
                else
                    await AddCustomerAsync();
            });
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
            await dataAccess.SaveCustomerAsync(newCustomer);
        }

        public Command SaveAllCommand
        {
            get
            {
                return new Command(async () =>
                    await dataAccess.
                    SaveAllCustomersAsync(Customers));
            }
        }

        public Command AddNewCommand
        {
            get
            {
                return new Command(async () =>
                    await AddCustomerAsync());
            }
        }

        public Command DeleteCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (SelectedCustomer != null)
                    {
                        Customers.Remove(SelectedCustomer);
                        await dataAccess.
                        DeleteCustomerAsync(SelectedCustomer);
                    }
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));
        }
    }
}
