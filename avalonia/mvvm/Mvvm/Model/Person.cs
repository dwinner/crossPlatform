using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mvvm.Model;

public class Person : INotifyPropertyChanged
{
   public string FullName
   {
      get;
      set
      {
         field = value;
         OnPropertyChanged();
      }
   } = string.Empty;

   public DateTime DateOfBirth
   {
      get;
      set
      {
         field = value;
         OnPropertyChanged();
      }
   }

   public string Address
   {
      get;
      set
      {
         field = value;
         OnPropertyChanged();
      }
   } = string.Empty;

   public event PropertyChangedEventHandler? PropertyChanged;

   private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
   {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
   }
}