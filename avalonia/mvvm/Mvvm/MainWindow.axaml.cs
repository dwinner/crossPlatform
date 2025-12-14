using Avalonia.Controls;
using Mvvm.ViewModel;

namespace Mvvm;

public partial class MainWindow : Window
{
   public MainWindow()
   {
      InitializeComponent();

      ViewModel = new PersonViewModel();
      DataContext = ViewModel;
   }

   private PersonViewModel ViewModel { get; set; }
}