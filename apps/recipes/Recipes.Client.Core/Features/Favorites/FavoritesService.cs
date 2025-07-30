using CommunityToolkit.Mvvm.Messaging;
using Recipes.Client.Core.Messages;

namespace Recipes.Client.Core.Features.Favorites;

public class FavoritesService : IFavoritesService
{
   private readonly IFavoritesRepository _favoritesRepository;
   private readonly WeakReferenceMessenger _weakRefMessenger;

   // PERF: This kind of collection is not suitable enough for faster search operations
   private List<string>? _favorites;

   public FavoritesService(IFavoritesRepository favoritesRepository)
   {
      _favoritesRepository = favoritesRepository;
      _weakRefMessenger = WeakReferenceMessenger.Default;
   }

   public async Task<IReadOnlyCollection<string>?> LoadFavorites()
   {
      await LoadList();
      return _favorites?.AsReadOnly();
   }

   public async Task<bool> IsFavorite(string id)
   {
      await LoadList();
      return _favorites is not null && _favorites.Contains(id);
   }

   public async Task<Result<Nothing>> Add(string id)
   {
      var currentUserId = GetCurrentUserId();
      var result = await _favoritesRepository.Add(currentUserId, id);
      if (result.IsSuccess)
      {
         if (_favorites is not null && !_favorites.Contains(id))
         {
            _favorites.Add(id);
         }

         _weakRefMessenger.Send(new FavoriteUpdateMessage(id, true));
      }

      return result;
   }

   public async Task<Result<Nothing>> Remove(string id)
   {
      var currentUserId = GetCurrentUserId();
      var result = await _favoritesRepository.Remove(currentUserId, id);
      if (result.IsSuccess)
      {
         if (_favorites is not null && _favorites.Contains(id))
         {
            _favorites.Remove(id);
         }

         _weakRefMessenger.Send(new FavoriteUpdateMessage(id, false));
      }

      return result;
   }

   // FIXME: Dummy implementation, could be retrieved via injected 
   private string GetCurrentUserId() => "3";

   private async ValueTask LoadList()
   {
      if (_favorites is not null)
      {
         return;
      }

      var currentUserId = GetCurrentUserId();
      var loadResult = await _favoritesRepository.LoadFavorites(currentUserId);
      if (loadResult.IsSuccess)
      {
         _favorites = loadResult.Data.ToList();
      }
   }
}