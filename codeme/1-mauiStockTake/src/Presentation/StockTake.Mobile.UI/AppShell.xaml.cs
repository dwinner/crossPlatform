using MauiStockTake.UI.Helpers;
using MauiStockTake.UI.Pages;
using MauiStockTake.UI.Resources.Themes;

namespace MauiStockTake.UI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("productdetails", typeof(ProductPage));
        ThemeMenuItem.Text = "Switch to Sandy Theme";
    }

    private void ThemeMenuItem_Clicked(object sender, EventArgs e)
    {
        if (App.Theme == Theme.Default)
        {
            App.Theme = Theme.Sandy;
            ThemeMenuItem.Text = "Switch to Default Theme";
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
                mergedDictionaries.Add(new SandyTheme());
            }
        }
        else
        {
            App.Theme = Theme.Default;
            ThemeMenuItem.Text = "Switch to Sandy Theme";
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
                mergedDictionaries.Add(new DefaultTheme());
            }
        }

        MessagingCenter.Send<AppShell>(this, "ThemeChanged");
    }
}