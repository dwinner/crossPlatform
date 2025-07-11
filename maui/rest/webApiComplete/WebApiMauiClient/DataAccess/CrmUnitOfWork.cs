namespace c4_LocalDatabaseConnection.DataAccess;

public class CrmUnitOfWork : IDisposable, IUnitOfWork<Customer>
{
   private readonly ICacheService _cacheService = MemoryCacheService.Instance;
   private IRepository<Customer> _customerRepository;

   public void Dispose()
   {
   }

   public IRepository<Customer> Items =>
      _customerRepository ??= new CustomersCachedRepository(new CustomerWebRepository(), _cacheService);

   public async Task SaveAsync()
   {
      _cacheService.ExecuteCacheUpdateActions();
      await Task.CompletedTask;
   }
}