using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Calculator.ViewModels;

public partial class MainPageViewModel(Calculations results, IMessenger messenger)
{
   public Calculations Results { get; init; } = results;

   [RelayCommand]
   public void Recall(Calculation sender)
   {
      messenger.Send(sender);
   }
}