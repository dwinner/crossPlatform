using System.Diagnostics;

namespace c7_CustomizeHandler;

public partial class MainPage
{
   public MainPage()
   {
      InitializeComponent();
   }

   private void OnEntryHandlerChanged(object sender, EventArgs e)
   {
      var entry = sender as Entry;
      Debug.Assert(entry?.Handler?.PlatformView != null);
      PlatformCustomizer.CustomizeEntry(entry.Handler.PlatformView);
   }
}

public static partial class PlatformCustomizer
{
   public static partial void CustomizeEntry(object platformView);
}