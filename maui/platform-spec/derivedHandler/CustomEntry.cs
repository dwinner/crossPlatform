namespace c7_DerivedHandler;

public class CustomEntry : Entry
{
   public static readonly BindableProperty SelectionColorProperty = BindableProperty.Create(
      nameof(SelectionColor),
      typeof(Color),
      typeof(CustomEntry),
      Colors.Gray);

   public Color SelectionColor
   {
      get => (Color)GetValue(SelectionColorProperty);
      set => SetValue(SelectionColorProperty, value);
   }
}