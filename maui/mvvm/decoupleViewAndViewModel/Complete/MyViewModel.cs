using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace c2_DecoupleViewAndViewModel;

public class MainViewModel : INotifyPropertyChanged
{
   private int _count;
   private string _textValue = "Click Me!";

   public MainViewModel() => UpdateTextCommand = new Command(UpdateText);

   public string TextValue
   {
      get => _textValue;
      set
      {
         _textValue = value;
         OnPropertyChanged();
      }
   }

   public ICommand UpdateTextCommand { get; set; }

   public event PropertyChangedEventHandler? PropertyChanged;

   public void UpdateText()
   {
      _count++;
      if (_count == 1)
      {
         TextValue = $"Clicked {_count} time";
      }
      else
      {
         TextValue = $"Clicked {_count} times";
      }
   }

   protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
   {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
   }
}