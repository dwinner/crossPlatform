using System.Diagnostics;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using Font = Microsoft.Maui.Font;

namespace CrossPlatformCapabilities;

public partial class MainPage
{
   private readonly WeakReferenceMessenger _messenger = WeakReferenceMessenger.Default;

   public MainPage()
   {
      InitializeComponent();

      _messenger.Register<ConnectionChangedMessage>(
         this,
         async (_, message) =>
         {
            await ShowConnectionSnackbarAsync(message.Value).ConfigureAwait(true);
         });

      _messenger.Register<BatteryStatusChangedMessage>(
         this,
         (_, _) =>
         {
            ManageBatteryLevelChanged();
         });
   }

   private async Task ShowConnectionSnackbarAsync(bool value)
   {
      var options = new SnackbarOptions
      {
         BackgroundColor = Colors.PaleVioletRed,
         TextColor = Colors.White,
         ActionButtonTextColor = Colors.White,
         CornerRadius = new CornerRadius(10),
         Font = Font.SystemFontOfSize(14)
      };

      var message = value switch
      {
         true => "Internet connection available",
         _ => "Internet connection lost"
      };

      await this.DisplaySnackbar(message, visualOptions: options).ConfigureAwait(true);
      Debug.WriteLine(nameof(ShowConnectionSnackbarAsync));
   }

   private void ManageBatteryLevelChanged()
   {
      FileHelper.WriteData("test data");
   }
}