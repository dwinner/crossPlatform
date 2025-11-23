using MauiStockTake.UI.Helpers;
using MauiStockTake.UI.Pages;

namespace MauiStockTake.UI;

public partial class App : Application
{
    private bool _loggedIn;

    public static Theme Theme { get; set; } = Theme.Default;

    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }

    protected override async void OnStart()
    {
        base.OnStart();

        if (!_loggedIn)
        {
            await MainPage.Navigation.PushModalAsync<LoginPage>();
            _loggedIn = true;
        }
    }
}