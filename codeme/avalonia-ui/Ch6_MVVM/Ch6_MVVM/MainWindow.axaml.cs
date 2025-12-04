using Avalonia.Controls;
using Avalonia.Interactivity;
using Ch6_MVVM.ViewModel;

namespace Ch6_MVVM
{
    public partial class MainWindow : Window
    {
        private PersonViewModel ViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            ViewModel=new PersonViewModel();
            this.DataContext= ViewModel;
        }
    }
}