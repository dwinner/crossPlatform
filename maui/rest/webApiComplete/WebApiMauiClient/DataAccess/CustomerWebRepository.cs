using c4_LocalDatabaseConnection.HttpCommunication;

namespace c4_LocalDatabaseConnection.DataAccess;

public class CustomerWebRepository : IRepository<Customer>
{
   private readonly string _collectionName = "Customers";
   private readonly HttpClient _httpClient = WebApiHttpClient.Instance;

   public async Task<IEnumerable<Customer>> GetAllAsync()
   {
      var response = await _httpClient.GetAsync(_collectionName);
      response.EnsureSuccessStatusCode();
      return await response.Content.ReadAsAsync<IEnumerable<Customer>>();
   }

   public async Task AddAsync(Customer item)
   {
      var response = await _httpClient.PostAsJsonAsync(_collectionName, item);
      response.EnsureSuccessStatusCode();
   }

   public async Task DeleteAsync(Customer item)
   {
      var response = await _httpClient.DeleteAsync($"{_collectionName}/{item.Id}");
      response.EnsureSuccessStatusCode();
   }

   public async Task<Customer> GetByIdAsync(int id)
   {
      var response = await _httpClient.GetAsync($"{_collectionName}/{id}");
      response.EnsureSuccessStatusCode();
      return await response.Content.ReadAsAsync<Customer>();
   }

   public async Task UpdateAsync(Customer item)
   {
      var response = await _httpClient.PutAsJsonAsync($"{_collectionName}/{item.Id}", item);
      response.EnsureSuccessStatusCode();
   }
}