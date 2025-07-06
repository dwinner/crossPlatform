using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace c2_ViewModelCommunication.ViewModels;

public partial class SecurityMonitorViewModel : ObservableObject
{
   private readonly WeakReferenceMessenger _messenger = WeakReferenceMessenger.Default;
   [ObservableProperty] private ObservableCollection<string> _securityAlerts;

   public SecurityMonitorViewModel()
   {
      _securityAlerts = new ObservableCollection<string>();
      _messenger.Register<AlertMessage, string>(this, AlertTypes.Security,
         (_, alert) =>
         {
            if (alert.Value != null)
            {
               SecurityAlerts.Add(alert.Value);
            }
         });
   }
}

/*public class RequestAlert : RequestMessage<string>;*/