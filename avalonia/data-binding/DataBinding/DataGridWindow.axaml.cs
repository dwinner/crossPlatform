using Avalonia.Controls;
using Ch6_Resources_DataBinding;
using System.Collections.ObjectModel;

namespace Ch6_Resources_and_DataBinding
{
    public partial class DataGridWindow : Window
    {
        public DataGridWindow()
        {
            InitializeComponent();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); 

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

        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the current item
            var currentItem = DataGrid1.SelectedItem as Person;

            // Get the first selected person from the collection
            var selectedPerson = e.AddedItems[0] as Person;

        }

        private void DataGrid1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                // Further data validation here...
            }
        }

        private void DataGrid1_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if(e.EditAction == DataGridEditAction.Commit)
            {
                // Some post-save actions here...
            }
        }
    }
}
