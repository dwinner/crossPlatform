using Avalonia.Controls;

namespace Ch6_Resources_DataBinding
{
    public partial class SimpleBindingWindow : Window
    {
        public SimpleBindingWindow()
        {
            InitializeComponent();
            Person person = new Person();
            this.DataContext = person;

        }
    }
}
