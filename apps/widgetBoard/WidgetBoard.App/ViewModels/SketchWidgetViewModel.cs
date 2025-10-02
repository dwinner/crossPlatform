namespace WidgetBoard.App.ViewModels;

public class SketchWidgetViewModel : IWidgetViewModel
{
   private const string DisplayName = "Sketch";

   public int Position { get; set; }

   public string Type => DisplayName;
}