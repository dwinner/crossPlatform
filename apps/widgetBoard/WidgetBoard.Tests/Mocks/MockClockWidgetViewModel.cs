using WidgetBoard.App.ViewModels;

namespace WidgetBoard.Tests.Mocks;

public class MockClockWidgetViewModel(DateTime time) : IWidgetViewModel
{
   public DateTime Time { get; } = time;

   public int Position { get; set; }

   public string Type => "Mock";
}