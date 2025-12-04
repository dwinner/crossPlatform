using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Data;

namespace Ch5_Controls
{
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
            switch(Button3.IsChecked)
            {
                case true:
                    // Take an action
                    break;
                default: 
                    // Take an action
                    break;
            }
        }

        private void ButtonSpinner1_Spin(object sender, SpinEventArgs e)
        {
            int content = Convert.ToInt32(ButtonSpinner1.Content);
            switch (e.Direction)
            {
                case SpinDirection.Increase:
                    content++;
                    ButtonSpinner1.Content = content;
                    break;
                case SpinDirection.Decrease:
                    content--;
                    ButtonSpinner1.Content = content;
                    break;
            }
        }
    }
}
