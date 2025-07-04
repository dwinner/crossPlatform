using SQLite;

namespace LocalDataAccess;

public class DatabaseConnection
{
   public const string DatabaseFilename = "Customers.db3";

   public const SQLiteOpenFlags Flags =
      // open the database in read/write mode
      SQLiteOpenFlags.ReadWrite |
      // create the database if it doesn't exist
      SQLiteOpenFlags.Create |
      // enable multi-threaded database access
      SQLiteOpenFlags.SharedCache;

   private SQLiteAsyncConnection _database;

   public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

   private async Task InitAsync()
   {
      if (_database is not null)
      {
         return;
      }

      _database = new SQLiteAsyncConnection(DatabasePath, Flags);
      _ = await _database.CreateTableAsync<Customer>().ConfigureAwait(true);
   }

   public async Task<List<Customer>> GetCustomersAsync()
   {
      await InitAsync().ConfigureAwait(true);
      var customers = await _database.Table<Customer>().ToListAsync()
         .ConfigureAwait(true);
      return customers;
   }

   public async Task<List<Customer>> GetFilteredCustomersAsync(string countryName)
   {
      var query = from cust in _database.Table<Customer>()
         where cust.Country == countryName
         select cust;

      //  await Database.QueryAsync<Customer>(
      //$"SELECT * FROM Item WHERE Country = '{countryName}'");

      var customersByCountry = await query.ToListAsync().ConfigureAwait(true);
      return customersByCountry;
   }

   public async Task<Customer> GetCustomerAsync(int id)
   {
      await InitAsync().ConfigureAwait(true);
      var customerById = await _database.Table<Customer>().Where(customer => customer.Id == id).FirstOrDefaultAsync()
         .ConfigureAwait(true);
      return customerById;
   }

   public async Task<int> SaveCustomerAsync(Customer aCustomer)
   {
      await InitAsync().ConfigureAwait(true);
      var affected = aCustomer.Id != 0
         ? await _database.UpdateAsync(aCustomer).ConfigureAwait(true)
         : await _database.InsertAsync(aCustomer).ConfigureAwait(true);
      return affected;
   }

   public async Task SaveAllCustomersAsync(IEnumerable<Customer> customers)
   {
      await InitAsync().ConfigureAwait(true);

      foreach (var customer in customers)
      {
         Func<object, Task<int>> affectFunc = customer.Id != 0
            ? customerObj => _database.UpdateAsync(customerObj)
            : customerObj => _database.InsertAsync(customerObj);
         await affectFunc(customer).ConfigureAwait(true);
      }
   }

   public async Task<int> DeleteCustomerAsync(Customer item)
   {
      await InitAsync().ConfigureAwait(true);
      var affected = await _database.DeleteAsync(item)
         .ConfigureAwait(true);
      return affected;
   }
}