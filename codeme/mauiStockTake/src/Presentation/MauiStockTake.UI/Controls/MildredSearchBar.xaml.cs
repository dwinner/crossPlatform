using System.Windows.Input;

namespace MauiStockTake.UI.Controls;

public partial class MildredSearchBar : ContentView
{
    // binmdable property for SearchCommand of type ICommand
    public static readonly BindableProperty SearchCommandProperty = BindableProperty.Create(nameof(SearchCommand), typeof(ICommand), typeof(MildredSearchBar), null);
    public ICommand SearchCommand { get; set; }


    // bindable property for AutoSearch of type bool
    public static readonly BindableProperty AutoSearchProperty = BindableProperty.Create(nameof(AutoSearch), typeof(bool), typeof(MildredSearchBar), false);
    public bool AutoSearch { get; set; }

    // bindable property for SearchText of type string
    public static readonly BindableProperty SearchTextProperty = BindableProperty.Create(nameof(SearchText), typeof(string), typeof(MildredSearchBar), string.Empty);
    public string SearchText { get; set; }


    public MildredSearchBar()
	{
		InitializeComponent();
        BindingContext = this;
	}

    private void Search_Tapped(object sender, TappedEventArgs e)
    {
        //SearchCommand.Execute();
    }
}