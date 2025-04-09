using CommunityToolkit.Mvvm.ComponentModel;

namespace DoToo.ViewModels;

[ObservableObject]
public abstract partial class ViewModel
{
   public INavigation? Navigation { get; set; }
}