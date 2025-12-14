using Avalonia.Controls;
using Ch6_Resources_DataBinding;
using System.Collections.ObjectModel;

namespace Ch6_Resources_and_DataBinding
{
    public partial class ItemsRepeaterWindow : Window
    {
        public ItemsRepeaterWindow()
        {
            InitializeComponent();

            Person person1 = new Person
            {
                FullName = "Alessandro",
                DateOfBirth = new System.DateTime(1977, 5, 10)
            };
            Person person2 = new Person
            {
                FullName = "James",
                DateOfBirth = new System.DateTime(1980, 1, 1)
            };
            Person person3 = new Person
            {
                FullName = "Graham",
                DateOfBirth = new System.DateTime(1982, 12, 31)
            };
            var people = new ObservableCollection<Person>() { person1, person2,
             person3 };

            this.DataContext = people;
        }
    }
}
