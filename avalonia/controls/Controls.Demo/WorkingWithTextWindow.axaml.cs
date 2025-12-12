using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Input;

namespace Controls.Demo;

public partial class WorkingWithTextWindow : Window
{
   public WorkingWithTextWindow()
   {
      InitializeComponent();
      foodBox.ItemsSource = new[] { "Pizza", "Caesar salad", "Hot dog", "Seafood" };
   }

   private void TextBox1_TextInput(object sender, TextInputEventArgs e)
   {
      var inputText = e.Text;
      Debug.WriteLine(inputText);
   }
}