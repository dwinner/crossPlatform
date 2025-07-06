using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace c2_ViewModelCommunication.ViewModels;

public partial class AlertGeneratorViewModel : ObservableObject
{
   private readonly WeakReferenceMessenger _messenger = WeakReferenceMessenger.Default;
   private int _alertCount;

   [ObservableProperty] private string? _alertText;

   [RelayCommand]
   public void GenerateAlert()
   {
      var channelType = ++_alertCount % 2 == 0
         ? AlertTypes.Security
         : AlertTypes.Performance;
      _messenger.Send(new AlertMessage(AlertText), channelType);
   }
}