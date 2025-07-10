namespace c3_DarkAndLightThemes;

public class BarChartDrawable : IDrawable
{
   private readonly float _cornerRadius = 4;
   private readonly Color[] _palette = [Colors.LightGreen, Colors.Gold, Colors.Coral];
   private readonly float _spacing = 5;
   public float Value { get; set; } = 1;

   public void Draw(ICanvas canvas, RectF dirtyRect)
   {
      canvas.SaveState();
      var rectSize = dirtyRect.Height;
      var maxStep = (int)(dirtyRect.Width / (rectSize + _spacing));
      var valueBasedSteps = (int)(maxStep * Value);

      for (var step = 0; step < valueBasedSteps; step++)
      {
         canvas.FillColor = _palette[_palette.Length * step / maxStep];
         canvas.FillRoundedRectangle(
            (rectSize + _spacing) * step,
            0,
            rectSize,
            rectSize,
            _cornerRadius
         );
      }

      canvas.RestoreState();
   }
}