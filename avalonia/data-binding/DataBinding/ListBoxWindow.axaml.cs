using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;

namespace DataBinding;

public partial class ListBoxWindow : Window
{
   public ListBoxWindow()
   {
      InitializeComponent();

      var person1 = new Person
      {
         FullName = "Alessandro",
         DateOfBirth = new DateTime(1977, 5, 10)
      };
      var person2 = new Person
      {
         FullName = "James",
         DateOfBirth = new DateTime(1980, 1, 1)
      };
      var person3 = new Person
      {
         FullName = "Graham",
         DateOfBirth = new DateTime(1982, 12, 31)
      };

      People = new People { Persons = [person1, person2, person3] };

      DataContext = People;
   }

   public People People
   {
      get;
      set;
   }
}

public class People
{
   public required ObservableCollection<Person> Persons { get; set; }
}