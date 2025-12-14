using Avalonia.Controls;

namespace DataBinding;

public partial class SimpleBindingWindow : Window
{
   public SimpleBindingWindow()
   {
      InitializeComponent();
      var person = new Person();
      DataContext = person;
   }
}