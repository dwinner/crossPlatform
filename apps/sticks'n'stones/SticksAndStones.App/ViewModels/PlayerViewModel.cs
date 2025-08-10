using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SticksAndStones.Models;
using SticksAndStones.Services;

namespace SticksAndStones.ViewModels;

public partial class PlayerViewModel(Player player, GameService gameService) : ObservableObject
{
   [ObservableProperty] [NotifyPropertyChangedFor(nameof(ChallengeStatus))]
   private bool isChallenging;

   public Guid Id => player.Id;

   public string GamerTag => player.GamerTag;

   public string EmailAddress => player.EmailAddress;

   public bool IsInMatch => !(player.MatchId == Guid.Empty);

   public string Status => IsInMatch switch
   {
      true => "In a game",
      false => "Waiting for opponent"
   };

   public string ChallengeStatus => IsChallenging switch
   {
      true => "Challenging...",
      false => "Challenge"
   };

   public bool CanChallenge => !IsInMatch && !IsChallenging;

   [RelayCommand(CanExecute = nameof(CanChallenge))]
   public void Challenge()
   {
      MainThread.BeginInvokeOnMainThread(async () =>
      {
         IsChallenging = true;
         var answer = await Shell.Current.CurrentPage.DisplayAlert(
            "Issue Challenge!",
            $"You are about to challenge {GamerTag} to a match!\nAre you sure?",
            "Yes",
            "No");
         if (answer)
         {
            await gameService.IssueChallenge(player);
         }

         IsChallenging = false;
      });
   }
}