namespace CertificatePinning;

public partial class MainPage
{
   private int _count;

   public MainPage()
   {
      InitializeComponent();
      _ = new HttpClientHandler();
   }

   private void OnCounterClicked(object sender, EventArgs e)
   {
      _count++;
      counterBtn.Text = _count == 1 ? $"Clicked {_count} time" : $"Clicked {_count} times";
      SemanticScreenReader.Announce(counterBtn.Text);
   }
}