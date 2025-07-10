namespace c3_DarkAndLightThemes;

public partial class SegmentedBarChartView
{
   public static readonly BindableProperty ValueProperty = BindableProperty.Create(
      nameof(Value),
      typeof(float),
      typeof(SegmentedBarChartView),
      0f,
      propertyChanged: (bindableObj, _, _) => ((SegmentedBarChartView)bindableObj).OnValueChanged()
   );

   public SegmentedBarChartView()
   {
      InitializeComponent();
   }

   public float Value
   {
      get => (float)GetValue(ValueProperty);
      set => SetValue(ValueProperty, value);
   }

   private void OnValueChanged()
   {
      ((BarChartDrawable)graphicsView.Drawable).Value = Value;
      graphicsView.Invalidate();
   }
}