using CommunityToolkit.Mvvm.ComponentModel;
using SticksAndStones.Models;

namespace SticksAndStones.ViewModels;

public partial class MatchPlayerViewModel(Player player, Match match) : ObservableObject
{
   public int PlayerToken => player.Id == match.PlayerOneId ? 1 : -1;

   public bool IsPlayersTurn => player.Id == match.NextPlayerId;

   public Guid Id => player.Id;

   public string GamerTag => player.GamerTag;

   public string EmailAddress => player.EmailAddress;

   public int Score => player.Id == match.PlayerOneId ? match.PlayerOneScore : match.PlayerTwoScore;
}