namespace WidgetBoard.App;

public class DrawingPath(Color color, float thickness)
{
   public Color Color { get; } = color;

   public PathF Path { get; } = new();

   public float Thickness { get; } = thickness;

   public void Add(PointF point) => Path.LineTo(point);
}