using Microsoft.Maui.Layouts;

namespace c1_CustomLayout;

public class CircularLayout : Layout
{
   public static readonly BindableProperty RadiusProperty = BindableProperty.Create(
      nameof(Radius),
      typeof(double),
      typeof(CircularLayout));

   public double Radius
   {
      get => (double)GetValue(RadiusProperty);
      set => SetValue(RadiusProperty, value);
   }

   protected override ILayoutManager CreateLayoutManager() => new CircularLayoutManager(this);

   private sealed class CircularLayoutManager(CircularLayout layout) : ILayoutManager
   {
      public Size Measure(double widthConstraint, double heightConstraint)
      {
         foreach (var child in layout)
         {
            if (child.Visibility == Visibility.Collapsed)
            {
               continue;
            }

            child.Measure(double.PositiveInfinity, double.PositiveInfinity);
         }

         return new Size(layout.WidthRequest, layout.HeightRequest);
      }

      public Size ArrangeChildren(Rect bounds)
      {
         var radius = layout.Radius;
         var angleStep = Math.PI * 2 / layout.Count;
         for (var i = 0; i < layout.Count; i++)
         {
            var child = layout[i];
            if (child.Visibility == Visibility.Collapsed)
            {
               continue;
            }

            child.Arrange(new Rect(
               radius * Math.Cos(angleStep * i) + radius,
               radius * Math.Sin(angleStep * i) + radius,
               child.DesiredSize.Width,
               child.DesiredSize.Height));
         }

         return new Size(layout.WidthRequest, layout.HeightRequest);
      }
   }
}