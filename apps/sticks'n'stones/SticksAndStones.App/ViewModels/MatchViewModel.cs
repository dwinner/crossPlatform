using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SticksAndStones.Messages;
using SticksAndStones.Models;
using SticksAndStones.Services;

namespace SticksAndStones.ViewModels;

public partial class MatchViewModel : ViewModelBase, IQueryAttributable
{
   private readonly GameService gameService;

   private int lastSelectedStick = -1;

   [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsCurrentPlayersTurn))]
   private Match match;

   [ObservableProperty] private MatchPlayerViewModel[] players;

   public MatchViewModel(GameService gameService)
   {
      this.gameService = gameService;
      IsActive = true;
   }

   public bool IsCurrentPlayersTurn => gameService.CurrentPlayer.Id == Match.NextPlayerId;

   public void ApplyQueryAttributes(IDictionary<string, object> query)
   {
      MainThread.InvokeOnMainThreadAsync(async () =>
      {
         Match lMatch = null;
         if (query.TryGetValue(Constants.ArgumentNames.Match, out var matchValue))
         {
            lMatch = matchValue as Match;
         }

         if (query.TryGetValue(Constants.ArgumentNames.MatchId, out var guidValue))
         {
            var matchId = new Guid($"{guidValue}");
            if (matchId != Guid.Empty)
            {
               lMatch = await gameService.GetMatchById(matchId);
            }
         }

         LoadMatch(lMatch);
      });
   }

   private void LoadMatch(Match aMatch)
   {
      if (aMatch is null)
      {
         return;
      }

      Players =
      [
         new MatchPlayerViewModel(gameService.GetPlayerById(aMatch.PlayerOneId), aMatch),
         new MatchPlayerViewModel(gameService.GetPlayerById(aMatch.PlayerTwoId), aMatch)
      ];

      Match = aMatch;
   }

   [RelayCommand(CanExecute = nameof(IsCurrentPlayersTurn))]
   private void SelectStick(string arg)
   {
      if (gameService.CurrentPlayer is null)
      {
         return;
      }

      if (int.TryParse(arg, out var pos))
      {
         pos--; // adjust for 0 based indexes
         if (lastSelectedStick != -1 && lastSelectedStick != pos)
         {
            Match.Sticks[lastSelectedStick] = 0;
         }

         if (Match.Sticks[pos] != 0)
         {
            return;
         }

         Match.Sticks[pos] = gameService.CurrentPlayer.Id == Match.PlayerOneId
            ? Players[0].PlayerToken
            : Players[1].PlayerToken;
         lastSelectedStick = pos;
         OnPropertyChanged(nameof(Match));
      }
   }

   [RelayCommand]
   private async Task Play()
   {
      if (lastSelectedStick == -1)
      {
         await Shell.Current.CurrentPage.DisplayAlert("Make a move", "You must make a move before you play.", "Ok");
         return;
      }

      if (await Shell.Current.CurrentPage.DisplayAlert("Make a move",
             "Are you sure this is the move you want, this can't be undone.", "Yes", "No"))
      {
         var (newMatch, error) = await gameService.EndTurn(Match.Id, lastSelectedStick);
         if (error is not null)
         {
            await Shell.Current.CurrentPage.DisplayAlert("Error in move", error, "Ok");
            return;
         }

         lastSelectedStick = -1;
      }
   }

   [RelayCommand]
   private async Task Undo()
   {
      if (lastSelectedStick != -1)
      {
         if (await Shell.Current.CurrentPage.DisplayAlert("Undo your move",
                "Are you sure you don't want to play this move?", "Yes", "No"))
         {
            OnPropertyChanging(nameof(Match));
            Match.Sticks[lastSelectedStick] = 0;
            OnPropertyChanged(nameof(Match));
            lastSelectedStick = -1;
         }
      }
   }

   [RelayCommand]
   private async Task Forfeit()
   {
      var returnToLobby = true;

      if (!Match.Completed)
      {
         returnToLobby = await Shell.Current.CurrentPage.DisplayAlert("W A I T",
            "Returning to the Lobby will forfeit your match, are you sure you want to do that?", "Yes", "No");
      }

      if (returnToLobby)
      {
         await Shell.Current.GoToAsync("///Lobby");
      }
   }

   protected override void OnActivated()
   {
      Messenger.Register(this, (MessageHandler<object, MatchUpdated>)OnMatchUpdated);
   }

   protected override void OnDeactivated()
   {
      Messenger.Unregister<MatchUpdated>(this);
   }

   private void OnMatchUpdated(object r, MatchUpdated m)
   {
      LoadMatch(m.Value);
      if (Match.WinnerId != Guid.Empty && Match.Completed)
      {
         MainThread.InvokeOnMainThreadAsync(async () =>
         {
            if (Match.WinnerId == gameService.CurrentPlayer.Id)
            {
               await Shell.Current.CurrentPage.DisplayAlert("Congratulations!",
                  "You are victorious!\nPress the back button to return to the lobby.", "Ok");
            }
            else
            {
               await Shell.Current.CurrentPage.DisplayAlert("Bummer!",
                  "You were defeated, better luck next time!\nPress the back button to return to the lobby.", "Ok");
            }
         });
      }
   }
}