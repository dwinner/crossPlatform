using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Controls.Demo;

public partial class ButtonsWindow : Window
{
   public ButtonsWindow()
   {
      InitializeComponent();
   }

   private void Button1_Click(object sender, RoutedEventArgs e)
   {
      var window = new WorkingWithTextWindow();
      window.Show();
   }

   private void Button3_Click(object sender, RoutedEventArgs e)
   {
      switch (button3.IsChecked)
      {
         case true:
            // Take an action
            break;
      }
   }

   private void ButtonSpinner1_Spin(object sender, SpinEventArgs e)
   {
      var content = Convert.ToInt32(buttonSpinner1.Content);
      switch (e.Direction)
      {
         case SpinDirection.Increase:
            content++;
            buttonSpinner1.Content = content;
            break;
         case SpinDirection.Decrease:
            content--;
            buttonSpinner1.Content = content;
            break;
         default:
            throw new ArgumentOutOfRangeException(nameof(e.Direction));
      }
   }
}