using System.Diagnostics;

namespace c3_AttachedBehavior;

public class DoubleTapToZoomBehavior : Behavior<Image>
{
   public static readonly BindableProperty ScaleFactorProperty = BindableProperty.Create(
      nameof(ScaleFactor),
      typeof(double),
      typeof(DoubleTapToZoomBehavior),
      2d
   );

   private Image _image;
   private bool _isZoomed;
   private TapGestureRecognizer _tapGestureRecognizer;

   public double ScaleFactor
   {
      get => (double)GetValue(ScaleFactorProperty);
      set => SetValue(ScaleFactorProperty, value);
   }

   protected override void OnAttachedTo(Image bindable)
   {
      base.OnAttachedTo(bindable);
      _tapGestureRecognizer = new TapGestureRecognizer
      {
         NumberOfTapsRequired = 2
      };
      _image = bindable;
      _tapGestureRecognizer.Tapped += OnImageDoubleTap;
      _image.GestureRecognizers.Add(_tapGestureRecognizer);
   }

   protected override void OnDetachingFrom(Image bindable)
   {
      base.OnDetachingFrom(bindable);
      _image.GestureRecognizers.Remove(_tapGestureRecognizer);
      _tapGestureRecognizer.Tapped -= OnImageDoubleTap;
      _image = null;
   }

   private void OnImageDoubleTap(object sender, TappedEventArgs e)
   {
      var tappedPoint = e.GetPosition(_image);
      Debug.Assert(tappedPoint != null, $"{nameof(tappedPoint)} != null");
      
      if (_isZoomed)
      {
         _image.ScaleTo(1);
         _image.TranslateTo(0, 0);
      }
      else
      {
         var translateFactor = ScaleFactor - 1;
         var point = tappedPoint.Value;
         var translateX = (_image.Width / 2 - point.X) * translateFactor;
         var translateY = (_image.Height / 2 - point.Y) * translateFactor;
         _image.TranslateTo(translateX, translateY);
         _image.ScaleTo(ScaleFactor);
      }

      _isZoomed = !_isZoomed;
   }
}