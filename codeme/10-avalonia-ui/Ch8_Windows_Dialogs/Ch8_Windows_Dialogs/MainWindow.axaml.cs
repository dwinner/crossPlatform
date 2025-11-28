using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Ch8_Windows_Dialogs
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new SecondaryWindow();
            newWindow.Closed += NewWindow_Closed;
            newWindow.Show();
            // or
            //newWindow.ShowDialog(this);
        }

        private void NewWindow_Closed(object? sender, System.EventArgs e)
        {
            // Secondary window has been closed
        }
    }
}