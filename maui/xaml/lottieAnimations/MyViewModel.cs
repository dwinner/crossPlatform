using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c3_LottieAnimations;

public partial class MyViewModel : ObservableObject
{
   [ObservableProperty] private string _statusMessage = "Let's run!";

   [RelayCommand]
   private async Task HamsterRunAsync()
   {
      StatusMessage = "Running";
      await Task.Delay(TimeSpan.FromSeconds(5));
      StatusMessage = "Complete! Let's run again?";
   }
}