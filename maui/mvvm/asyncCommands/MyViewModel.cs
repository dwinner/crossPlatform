using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c2_DecoupleViewAndViewModel;

public partial class MainViewModel : ObservableObject
{
   [RelayCommand(IncludeCancelCommand = true)]
   public async Task UpdateTextAsync(CancellationToken token)
   {
      try
      {
         await Task.Delay(5000, token);
      }
      catch (OperationCanceledException)
      {
      }
      //other logic
   }
}