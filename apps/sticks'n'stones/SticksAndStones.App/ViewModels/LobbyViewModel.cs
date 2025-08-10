using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.Messaging;
using SticksAndStones.Messages;
using SticksAndStones.Models;
using SticksAndStones.Services;

namespace SticksAndStones.ViewModels;

public partial class LobbyViewModel : ViewModelBase
{
   private readonly GameService gameService;

   public LobbyViewModel(GameService gameService)
   {
      this.gameService = gameService;
      Players = new ObservableCollection<PlayerViewModel>(
         from player in gameService.Players
         where player.Id != gameService.CurrentPlayer.Id
         select new PlayerViewModel(player, gameService));
      CanRefresh = true;
      IsActive = true;
   }

   public ObservableCollection<PlayerViewModel> Players { get; init; }

   private void OnPlayersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
   {
      switch (e.Action)
      {
         case NotifyCollectionChangedAction.Add:
         {
            var newItems = e.NewItems;
            if (newItems != null)
            {
               foreach (var player in newItems.Cast<Player>())
               {
                  Players.Add(new PlayerViewModel(player, gameService));
               }
            }

            break;
         }
         case NotifyCollectionChangedAction.Remove:
         {
            var oldItems = e.OldItems;
            if (oldItems != null)
            {
               foreach (var player in oldItems.Cast<Player>())
               {
                  var toRemove = Players.FirstOrDefault(playerVm => playerVm.Id == player.Id);
                  Players.Remove(toRemove);
               }
            }

            break;
         }
         case NotifyCollectionChangedAction.Replace:
            break;
         case NotifyCollectionChangedAction.Reset:
            Players.Clear();
            break;
         case NotifyCollectionChangedAction.Move:
            break;
         default:
            throw new ArgumentOutOfRangeException(nameof(e));
      }
   }

   protected override void OnActivated()
   {
      Messenger.Register<ChallengeReceived>(this,
         (r, m) => OnChallengeReceived(m.Id, m.Value));
      Messenger.Register<MatchStarted>(this,
         (r, m) => OnMatchStarted(m.Value));
      Messenger.Register<ServiceError>(this,
         (_, m) => OnServiceError(m.Value));

      gameService.Players.CollectionChanged += OnPlayersCollectionChanged;

      // If the player has an in progress match, take them to it.
      if (gameService.CurrentPlayer?.MatchId != Guid.Empty)
      {
         MainThread.InvokeOnMainThreadAsync(async () =>
         {
            IsActive = false;
            await Shell.Current.GoToAsync("///Match",
               new Dictionary<string, object>
                  { { Constants.ArgumentNames.MatchId, gameService.CurrentPlayer.MatchId } });
         });
      }
   }

   protected override void OnDeactivated()
   {
      gameService.Players.CollectionChanged -= OnPlayersCollectionChanged;

      Messenger.Unregister<ChallengeReceived>(this);
      Messenger.Unregister<MatchStarted>(this);
      Messenger.Unregister<ServiceError>(this);
   }

   private void OnChallengeReceived(Guid challengeId, Player opponent)
   {
      MainThread.BeginInvokeOnMainThread(async () =>
      {
         var answer = await Shell.Current.CurrentPage.DisplayAlert(
            "You have been challenged!",
            $"{opponent.GamerTag} has challenged you to a match of Sticks & Stones, do you accept?",
            "Yes",
            "No");
         await gameService.SendChallengeResponse(challengeId,
            answer
               ? ChallengeResponse.Accepted
               : ChallengeResponse.Declined);
      });
   }

   private void OnMatchStarted(Match match)
   {
      MainThread.BeginInvokeOnMainThread(async () =>
      {
         IsActive = false;
         await Shell.Current.GoToAsync("///Match",
            new Dictionary<string, object> { { Constants.ArgumentNames.Match, match } });
      });
   }

   private void OnServiceError(AsyncError error)
   {
      MainThread.BeginInvokeOnMainThread(async () =>
      {
         IsActive = false;
         await Shell.Current.CurrentPage.DisplayAlert("There is a problem...", error.Message, "Ok");
      });
   }

   protected override async Task RefreshInternal()
   {
      await gameService.RefreshPlayerList();
   }
}