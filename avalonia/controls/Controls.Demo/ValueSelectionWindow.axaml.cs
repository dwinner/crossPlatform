using Avalonia;
using Avalonia.Controls;

namespace Controls.Demo;

public partial class ValueSelectionWindow : Window
{
   public ValueSelectionWindow()
   {
      InitializeComponent();
   }

   private void Slider1_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
   {
      if (e.Property.Name == nameof(slider1.Value))
      {
         // Handle Value here...
      }
   }

   private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
   {
      _ = e.AddedItems;
   }
}