using System.Globalization;
using WidgetBoard.App.Views;
using WidgetBoard.Tests.Mocks;

namespace WidgetBoard.Tests.Views;

public class ClockWidgetViewTests
{
   [Fact]
   public void TextIsUpdatedByTimeProperty()
   {
      var time = new DateTime(2022, 01, 01);
      var clockWidget = new ClockWidgetView();
      Assert.True(string.IsNullOrWhiteSpace(clockWidget.Text));

      clockWidget.WidgetViewModel = new MockClockWidgetViewModel(time);

      Assert.Equal(time.ToString(CultureInfo.InvariantCulture), clockWidget.Text.Trim());
   }
}