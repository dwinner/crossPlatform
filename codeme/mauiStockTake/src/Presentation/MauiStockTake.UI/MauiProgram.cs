using MauiStockTake.UI.Helpers;
using MauiStockTake.UI.Pages;
using MauiStockTake.Client;
using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;

namespace MauiStockTake.UI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UsePageResolver();

        builder.Services.AddSingleton<IBrowser, AuthBrowser>();

        builder.Services.AddSingleton<IAuthService, AuthService>();

        builder.Services.AddTransient<LoginPage>();

        builder.Services.AddTransient<InputPage>();

        builder.Services.AddTransient<ReportPage>();

        builder.Services.AddApiClientServices(new ApiClientOptions 
        { 
            BaseUrl = Constants.AuthorityUri
        });

        builder.Services.AddTransient<InputViewModel>();

        builder.Services.AddTransient<ReportViewModel>();

        return builder.Build();
    }
}