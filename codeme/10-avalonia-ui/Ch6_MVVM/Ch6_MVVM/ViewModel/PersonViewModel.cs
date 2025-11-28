using Ch6_MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ch6_MVVM.ViewModel
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Person> People { get; set; }

        private Person _selectedPerson;
        public Person SelectedPerson
        {
            get
            {
                return _selectedPerson;
            }
            set
            {
                _selectedPerson = value;
                OnPropertyChanged();
            }
        }

        public void AddPerson()
        {
            People.Add(new Person());
        }

        public bool CanAddPerson()
        {
            return true;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName
                                        = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }
        private void LoadSampleData()
        {
            People = new ObservableCollection<Person>();

            // sample data
            Person person1 =
                new Person
                {
                    FullName = "Alessandro",
                    Address = "Italy",
                    DateOfBirth = new DateTime(1977, 5, 10)
                };
            Person person2 =
                new Person
                {
                    FullName = "Robert",
                    Address = "United States",
                    DateOfBirth = new DateTime(1960, 2, 1)
                };
            Person person3 =
                new Person
                {
                    FullName = "Niklas",
                    Address = "Germany",
                    DateOfBirth = new DateTime(1980, 4, 2)
                };

            People.Add(person1);
            People.Add(person2);
            People.Add(person3);
        }

        public PersonViewModel()
        {
            LoadSampleData();
        }

    }
}
