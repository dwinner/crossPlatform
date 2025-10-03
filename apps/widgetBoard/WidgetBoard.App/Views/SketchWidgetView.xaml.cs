using WidgetBoard.App.ViewModels;

namespace WidgetBoard.App.Views;

public partial class SketchWidgetView : IWidgetView, IDrawable
{
   private readonly List<DrawingPath> _paths = new();
   private DrawingPath? _currentPath;

   public SketchWidgetView()
   {
      InitializeComponent();
      Drawable = this;
   }

   public void Draw(ICanvas canvas, RectF dirtyRect)
   {
      foreach (var path in _paths)
      {
         canvas.StrokeColor = path.Color;
         canvas.StrokeSize = path.Thickness;
         canvas.StrokeLineCap = LineCap.Round;
         canvas.DrawPath(path.Path);
      }
   }

   public IWidgetViewModel WidgetViewModel
   {
      get => (IWidgetViewModel)BindingContext;
      set => BindingContext = value;
   }

   private void OnGraphicsViewStartInteraction(object sender, TouchEventArgs e)
   {
      _currentPath = new DrawingPath(Colors.Black, 2);
      _currentPath.Add(e.Touches.First());
      _paths.Add(_currentPath);
      Invalidate();
   }

   private void OnGraphicsViewDragInteraction(object sender, TouchEventArgs e)
   {
      if (_currentPath is null)
      {
         return;
      }

      _currentPath.Add(e.Touches.First());
      Invalidate();
   }

   private void OnGraphicsViewEndInteraction(object sender, TouchEventArgs e)
   {
      if (_currentPath is null)
      {
         return;
      }

      _currentPath.Add(e.Touches.First());
      Invalidate();
   }
}