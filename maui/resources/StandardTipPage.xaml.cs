using System.Diagnostics;

namespace TipCalculator;

public partial class StandardTipPage
{
   private readonly Color _colorNavy = Colors.Navy;
   private readonly Color _colorSilver = Colors.Silver;

   public StandardTipPage()
   {
      InitializeComponent();
      billInput.TextChanged += (_, _) => CalculateTip();
   }

   private void CalculateTip()
   {
      if (!double.TryParse(billInput.Text, out var bill) || !(bill > 0))
      {
         return;
      }

      var tip = Math.Round(bill * 0.15, 2);
      var final = bill + tip;

      tipOutput.Text = tip.ToString("C");
      totalOutput.Text = final.ToString("C");
   }

   private void OnLight(object sender, EventArgs e)
   {
      Resources["fgColor"] = _colorNavy;
      Resources["bgColor"]=_colorSilver;
   }

   private void OnDark(object sender, EventArgs e)
   {
      Resources["fgColor"] = _colorSilver;
      Resources["bgColor"] = _colorNavy;
   }

   private async void GoToCustom(object sender, EventArgs e)
   {
      await Shell.Current.GoToAsync(nameof(CustomTipPage))
         .ConfigureAwait(true);
      Debug.WriteLine(nameof(GoToCustom));
   }
}