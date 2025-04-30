using SQLite;

namespace LocalDataAccess
{
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

        public static string DatabasePath
        {
            get
            {
                return Path.Combine(FileSystem.AppDataDirectory,
                    DatabaseFilename);
            }
        }

        SQLiteAsyncConnection Database;

        public DatabaseConnection()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;
                        
            Database = new SQLiteAsyncConnection(DatabasePath, Flags);
            CreateTableResult result = await Database.CreateTableAsync<Customer>();
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            await Init();
            return await Database.Table<Customer>().ToListAsync();
        }

        public async Task<List<Customer>> GetFilteredCustomersAsync
            (string countryName)
        {
            var query = from cust in Database.Table<Customer>()
                        where cust.Country == countryName
                        select cust;

          //  await Database.QueryAsync<Customer>(
          //$"SELECT * FROM Item WHERE Country = '{countryName}'");

            return await query.ToListAsync();
        }

        public async Task<Customer> GetCustomerAsync(int id)
        {
            await Init();
            return await Database.Table<Customer>().
                Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveCustomerAsync(Customer item)
        {
            await Init();
            if (item.Id != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task SaveAllCustomersAsync(IEnumerable<Customer> items)
        {
            await Init();
            foreach(var item in items)
            {
                if (item.Id != 0)
                    await Database.UpdateAsync(item);
                else
                    await Database.InsertAsync(item);
            }
        }

        public async Task<int> DeleteCustomerAsync(Customer item)
        { 
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
