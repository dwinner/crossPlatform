using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Mvvm.Model;

namespace Mvvm.ViewModel;

public class PersonViewModel : INotifyPropertyChanged
{
   public PersonViewModel()
   {
      LoadSampleData();
   }

   public ObservableCollection<Person>? People { get; set; }

   public Person? SelectedPerson
   {
      get;
      set
      {
         field = value;
         OnPropertyChanged();
      }
   }

   public event PropertyChangedEventHandler? PropertyChanged;

   public void AddPerson()
   {
      People?.Add(new Person());
   }

   public bool CanAddPerson() => true;

   private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
   {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
   }

   private void LoadSampleData()
   {
      People = [];

      // sample data
      var person1 = new Person
         {
            FullName = "Alessandro",
            Address = "Italy",
            DateOfBirth = new DateTime(1977, 5, 10)
         };
      var person2 = new Person
         {
            FullName = "Robert",
            Address = "United States",
            DateOfBirth = new DateTime(1960, 2, 1)
         };
      var person3 = new Person
         {
            FullName = "Niklas",
            Address = "Germany",
            DateOfBirth = new DateTime(1980, 4, 2)
         };

      People.Add(person1);
      People.Add(person2);
      People.Add(person3);
   }
}