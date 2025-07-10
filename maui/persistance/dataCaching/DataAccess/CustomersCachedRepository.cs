namespace c4_LocalDatabaseConnection.DataAccess;

public class CustomersCachedRepository(IRepository<Customer> innerRepository, ICacheService cacheService)
   : IRepository<Customer>
{
   private readonly string _collectionName = "customers";

   public async Task<IEnumerable<Customer>> GetAllAsync()
   {
      if (!cacheService.TryGetValue(_collectionName, out var customers))
      {
         customers = await innerRepository.GetAllAsync();
         cacheService.Set(_collectionName, customers);
      }

      return (IEnumerable<Customer>)customers;
   }

   public async Task AddAsync(Customer item)
   {
      await innerRepository.AddAsync(item);
      cacheService.AddPendingAction(new CollectionCacheUpdate(_collectionName, cachedList => cachedList.Add(item)));
   }

   public async Task DeleteAsync(Customer item)
   {
      await innerRepository.DeleteAsync(item);
      cacheService.AddPendingAction(new CollectionCacheUpdate(_collectionName, cachedList => cachedList.Remove(item)));
   }

   public async Task UpdateAsync(Customer item)
   {
      await innerRepository.UpdateAsync(item);
      cacheService.AddPendingAction(new CollectionCacheUpdate(_collectionName, cachedList =>
      {
         var editedItemIndex = ((List<Customer>)cachedList).FindIndex(c => c.Id == item.Id);
         cachedList[editedItemIndex] = item;
      }));
   }

   public async Task<Customer> GetByIdAsync(int id) => await innerRepository.GetByIdAsync(id);
}