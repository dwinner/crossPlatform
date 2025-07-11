using System.Collections;
using Microsoft.Extensions.Caching.Memory;

namespace c4_LocalDatabaseConnection.DataAccess;

public class MemoryCacheService : ICacheService
{
   private readonly MemoryCache _cache = new(new MemoryCacheOptions());
   private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(15);
   private readonly List<CollectionCacheUpdate> _pendingCacheUpdateActions = new();
   public static MemoryCacheService Instance { get; } = new();

   public void ClearCollectionCache(string collectionKey)
   {
      _cache.Remove(collectionKey);
   }

   public TItem Set<TItem>(object key, TItem value) => _cache.Set(key, value, _cacheDuration);

   public bool TryGetValue(object key, out object result) => _cache.TryGetValue(key, out result);

   public void AddPendingAction(CollectionCacheUpdate pendingAction)
   {
      _pendingCacheUpdateActions.Add(pendingAction);
   }

   public void ExecuteCacheUpdateActions()
   {
      foreach (var ca in _pendingCacheUpdateActions)
      {
         if (_cache.TryGetValue(ca.CollectionKey, out var cachedCollection))
         {
            ca.UpdateAction((IList)cachedCollection);
         }
      }

      _pendingCacheUpdateActions.Clear();
   }

   public void ClearCacheUpdateActions()
   {
      _pendingCacheUpdateActions.Clear();
   }
}

public class CollectionCacheUpdate(string collectionName, Action<IList> updateAction)
{
   public string CollectionKey { get; set; } = collectionName;
   public Action<IList> UpdateAction { get; set; } = updateAction;
}