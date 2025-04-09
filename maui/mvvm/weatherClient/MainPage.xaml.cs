using WeatherClient.Services;

namespace WeatherClient;

public partial class MainPage
{
   public MainPage()
   {
      InitializeComponent();
   }

   private async void OnRefresh(object sender, EventArgs e)
   {
      try
      {
         btnRefresh.IsEnabled = false;
         actIsBusy.IsRunning = true;
         BindingContext = await WeatherServer.GetWeather(txtPostalCode.Text)
            .ConfigureAwait(true);
      }
      finally
      {
         btnRefresh.IsEnabled = true;
         actIsBusy.IsRunning = false;
      }
   }
}