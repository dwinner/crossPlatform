using Avalonia.Controls;
using Avalonia.Interactivity;

namespace WinDialogs;

public partial class MainWindow : Window
{
   public MainWindow()
   {
      InitializeComponent();
   }

   private void OpenWindowButton_Click(object? sender, RoutedEventArgs e)
   {
      var newWindow = new SecondaryWindow();
      //newWindow.Closed += NewWindow_Closed;
      newWindow.Show();
   }
}