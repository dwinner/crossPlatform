using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StockTake.Mobile.UI.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
   public event PropertyChangedEventHandler PropertyChanged;

   private string _title;

   public string Title
   {
      get => _title;
      set
      {
         _title = value;
         OnPropertyChanged();
      }
   }

   private bool _isLoading;

   public bool IsLoading
   {
      get => _isLoading;
      set
      {
         _isLoading = value;
         OnPropertyChanged();
      }
   }

   protected bool initialised = false;

   public INavigation Navigation { get; set; }

   protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
   {
      var changed = PropertyChanged;
      if (changed == null)
         return;

      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
   }
}