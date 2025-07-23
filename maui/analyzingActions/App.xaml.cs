using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace AnalyzingActions;

public partial class App
{
   public App()
   {
      InitializeComponent();
   }


   protected override Window CreateWindow(IActivationState activationState) => new(new NavigationPage(new MainPage()));

   protected override void OnStart()
   {
      base.OnStart();
      AppCenter.Start("android=YOUR-APP-SECRET-GOES-HERE;",
         typeof(Analytics), typeof(Crashes));
   }
}