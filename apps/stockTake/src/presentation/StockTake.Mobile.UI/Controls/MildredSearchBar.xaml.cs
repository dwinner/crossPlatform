using System.Windows.Input;

namespace StockTake.Mobile.UI.Controls;

public partial class MildredSearchBar : ContentView
{
   // binmdable property for SearchCommand of type ICommand
   public static readonly BindableProperty SearchCommandProperty =
      BindableProperty.Create(nameof(SearchCommand), typeof(ICommand), typeof(MildredSearchBar));


   // bindable property for AutoSearch of type bool
   public static readonly BindableProperty AutoSearchProperty =
      BindableProperty.Create(nameof(AutoSearch), typeof(bool), typeof(MildredSearchBar), false);

   // bindable property for SearchText of type string
   public static readonly BindableProperty SearchTextProperty =
      BindableProperty.Create(nameof(SearchText), typeof(string), typeof(MildredSearchBar), string.Empty);


   public MildredSearchBar()
   {
      InitializeComponent();
      BindingContext = this;
   }

   public ICommand SearchCommand { get; set; }

   public bool AutoSearch { get; set; }

   public string SearchText { get; set; }

   private void Search_Tapped(object sender, TappedEventArgs e)
   {
      //SearchCommand.Execute();
   }
}