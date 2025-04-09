using Avalonia.Controls;
using Avalonia.Interactivity;

namespace GetStartedApp.Views;

public partial class MainWindow : Window
{
   public MainWindow()
   {
      InitializeComponent();
   }

   private void ButtonClicked(object source, RoutedEventArgs args) => UpdateUi();

   private void OnCelsius_TextChanged(object? sender, TextChangedEventArgs e) => UpdateUi();

   private void UpdateUi()
   {
      if (double.TryParse(celsiusTextBox.Text, out var celsiusVal))
      {
         var fahrVal = celsiusVal * (9D / 5D) + 32;
         fahrenheitTextBox.Text = fahrVal.ToString("0.0");
      }
      else
      {
         celsiusTextBox.Text = fahrenheitTextBox.Text = 0.ToString();
      }
   }
}