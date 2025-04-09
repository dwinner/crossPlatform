using MvvmDemo.Core;
using MvvmDemo.Impl;

namespace MvvmDemo;

public partial class MainPage
{
   public MainPage()
   {
      InitializeComponent();
      BindingContext = new MainPageViewModel(new QuoteService());
   }
}