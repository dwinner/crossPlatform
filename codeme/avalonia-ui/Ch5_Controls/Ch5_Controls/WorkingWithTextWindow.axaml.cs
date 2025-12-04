using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Ch5_Controls
{
    public partial class WorkingWithTextWindow : Window
    {
        public WorkingWithTextWindow()
        {
            InitializeComponent();
            FoodBox.Items = new string[] { "Pizza", "Caesar salad", "Hot dog", "Seafood" };
        }

        private void TextBox1_TextInput(object sender, TextInputEventArgs e)
        {
            string inputText = e.Text;
        }
    }
}
