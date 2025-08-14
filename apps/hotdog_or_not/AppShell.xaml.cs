using HotdogOrNot.Views;

namespace HotdogOrNot;

public partial class AppShell
{
   public AppShell()
   {
      Routing.RegisterRoute("Result", typeof(ResultView));
      InitializeComponent();
   }
}