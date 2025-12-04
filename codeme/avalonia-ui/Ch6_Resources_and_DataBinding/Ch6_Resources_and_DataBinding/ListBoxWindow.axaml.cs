using Avalonia.Controls;
using System.Collections.ObjectModel;

namespace Ch6_Resources_DataBinding
{
    public partial class ListBoxWindow : Window
    {
        public ListBoxWindow()
        {
            InitializeComponent();
            Person person1 = new Person { FullName = "Alessandro", 
                DateOfBirth = new System.DateTime(1977,5,10) };
            Person person2 = new Person { FullName = "James", 
                DateOfBirth = new System.DateTime(1980, 1, 1) };
            Person person3 = new Person { FullName = "Graham", 
                DateOfBirth = new System.DateTime(1982, 12, 31) };
            var people = new ObservableCollection<Person>() { person1, person2,
             person3 };

            this.DataContext = people;

        }

        //private void PeopleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
            //// Get the current item
            //var currentItem = PeopleList.SelectedItem as Person; 

            //// Get the first selected person from the collection
            //var selectedPerson = e.AddedItems[0] as Person;
        //}
    }
}
