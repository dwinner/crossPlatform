using Astronomy.Pages;

namespace Astronomy;

public partial class AppShell
{
   public AppShell()
   {
      InitializeComponent();

      Routing.RegisterRoute("astronomicalbodydetails", typeof(AstronomicalBodyPage));
   }
}