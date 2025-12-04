using Avalonia.Controls;
using Avalonia.Interactivity;
using Ch6_Resources_DataBinding;
using System.Collections.ObjectModel;

namespace Ch6_Resources_and_DataBinding
{
    public partial class CarouselWindow : Window
    {
        public CarouselWindow()
        {
            InitializeComponent();

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            Person person1 = new Person
            {
                FullName = "Alessandro",
                DateOfBirth = new System.DateTime(1977, 5, 10),
                Address = "Italy"
            };
            Person person2 = new Person
            {
                FullName = "James",
                DateOfBirth = new System.DateTime(1980, 1, 1),
                Address = "Washington"
            };
            Person person3 = new Person
            {
                FullName = "Graham",
                DateOfBirth = new System.DateTime(1982, 12, 31),
                Address = "Tennessee"
            };
            var people = new ObservableCollection<Person>() { person1, person2,
             person3 };

            this.DataContext = people;
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            Carousel1.Previous();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Carousel1.Next();
        }
    }
}
