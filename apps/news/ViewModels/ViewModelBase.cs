using CommunityToolkit.Mvvm.ComponentModel;

namespace News.ViewModels;

[ObservableObject]
public abstract partial class ViewModelBase
{
   internal ViewModelBase(INavigate navigation) => Navigation = navigation;

   public INavigate Navigation { get; init; }
}