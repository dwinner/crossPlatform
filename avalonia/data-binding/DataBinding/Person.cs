using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataBinding;

public class Person : INotifyPropertyChanged
{
   private string _fullName = string.Empty;
   private DateTime _dateOfBirth;
   private string _address = string.Empty;

   public string FullName
   {
      get => _fullName;
      set
      {
         _fullName = value;
         OnPropertyChanged();
      }
   }

   public DateTime DateOfBirth
   {
      get => _dateOfBirth;
      set
      {
         _dateOfBirth = value;
         OnPropertyChanged();
      }
   }

   public string Address
   {
      get => _address;
      set
      {
         _address = value;
         OnPropertyChanged();
      }
   }

   public event PropertyChangedEventHandler? PropertyChanged;

   private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
   {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
   }
}