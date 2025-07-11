namespace c4_LocalDatabaseConnection.DataAccess;

public class CustomersCachedRepository(CustomerWebRepository innerRepository, ICacheService cacheService)
   : IRepository<Customer>
{
   private readonly string _collectionName = "customers";
   private readonly IRepository<Customer> _innerRepository = innerRepository;

   public async Task<IEnumerable<Customer>> GetAllAsync()
   {
      if (!cacheService.TryGetValue(_collectionName, out var customers))
      {
         customers = await _innerRepository.GetAllAsync();
         cacheService.Set(_collectionName, customers);
      }

      return (IEnumerable<Customer>)customers;
   }

   public async Task AddAsync(Customer item)
   {
      await _innerRepository.AddAsync(item);
      cacheService.AddPendingAction(
         new CollectionCacheUpdate(_collectionName, cachedList => cachedList.Add(item)));
   }

   public async Task DeleteAsync(Customer item)
   {
      await _innerRepository.DeleteAsync(item);
      cacheService.AddPendingAction(
         new CollectionCacheUpdate(_collectionName, cachedList => cachedList.Remove(item)));
   }

   public async Task UpdateAsync(Customer item)
   {
      await _innerRepository.UpdateAsync(item);
      cacheService.AddPendingAction(
         new CollectionCacheUpdate(_collectionName, cachedList =>
         {
            var editedItemIndex = ((List<Customer>)cachedList).FindIndex(customer => customer.Id == item.Id);
            cachedList[editedItemIndex] = item;
         }));
   }

   public async Task<Customer> GetByIdAsync(int id) => await _innerRepository.GetByIdAsync(id);
}