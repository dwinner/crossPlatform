namespace WidgetBoard.App.ViewModels;

public class SketchWidgetViewModel : IWidgetViewModel
{
   internal const string DisplayName = "Sketch";

   public int Position { get; set; }

   public string Type => DisplayName;
}