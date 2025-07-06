using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c2_DecoupleViewAndViewModel;

public partial class MainViewModel : ObservableObject
{
   private int _count;

   [ObservableProperty] private string _textValue = "Click Me!";

   [RelayCommand]
   public void UpdateText()
   {
      _count++;
      TextValue = _count == 1 ? $"Clicked {_count} time" : $"Clicked {_count} times";
   }
}