using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace c2_ViewModelCommunication.ViewModels;

public partial class PerformanceMonitorViewModel : ObservableObject
{
   private readonly WeakReferenceMessenger _messenger = WeakReferenceMessenger.Default;

   [ObservableProperty] private ObservableCollection<string> _performanceAlerts;

   public PerformanceMonitorViewModel()
   {
      _performanceAlerts = new ObservableCollection<string>();
      _messenger.Register<AlertMessage, string>(this, AlertTypes.Performance, (_, alert) =>
      {
         var value = alert.Value;
         if (value != null)
         {
            PerformanceAlerts.Add(value);
         }
      });
   }
}