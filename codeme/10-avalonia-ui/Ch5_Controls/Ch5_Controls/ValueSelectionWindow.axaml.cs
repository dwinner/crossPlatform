using Avalonia;
using Avalonia.Controls;
using System.Linq;

namespace Ch5_Controls
{
    public partial class ValueSelectionWindow : Window
    {
        public ValueSelectionWindow()
        {
            InitializeComponent();
        }

        private void Slider1_PropertyChanged(object sender, 
            AvaloniaPropertyChangedEventArgs e)
        {
            if(e.Property.Name == nameof(Slider1.Value))
            {
                // Handle Value here...
            }
        }

        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItems = e.AddedItems;
        }
    }
}
