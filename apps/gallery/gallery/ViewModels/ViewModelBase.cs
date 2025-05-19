using CommunityToolkit.Mvvm.ComponentModel;

namespace Gallery.Core.ViewModels;

public abstract partial class ViewModelBase : ObservableObject
{
   private protected const int DefaultPhotoCount = 20;

   [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsNotBusy))]
   private bool _isBusy;

   public bool IsNotBusy => !IsBusy;

   protected internal abstract Task Initialize();
}