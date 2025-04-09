namespace TipCalculator;

public partial class MainPage
{
   public MainPage()
   {
      InitializeComponent();

      billInput.TextChanged += (s, e) => CalculateTip(false, false);
      roundDown.Clicked += (s, e) => CalculateTip(false, true);
      roundUp.Clicked += (s, e) => CalculateTip(true, false);

      tipPercentSlider.ValueChanged += (s, e) =>
      {
         var pct = Math.Round(e.NewValue);
         tipPercent.Text = pct + "%";
         CalculateTip(false, false);
      };
   }

   private void CalculateTip(bool roundUp, bool roundDown)
   {
      double t;
      if (double.TryParse(billInput.Text, out t) && t > 0)
      {
         var pct = Math.Round(tipPercentSlider.Value);
         var tip = Math.Round(t * (pct / 100.0), 2);

         var final = t + tip;

         if (roundUp)
         {
            final = Math.Ceiling(final);
            tip = final - t;
         }
         else if (roundDown)
         {
            final = Math.Floor(final);
            tip = final - t;
         }

         tipOutput.Text = tip.ToString("C");
         totalOutput.Text = final.ToString("C");
      }
   }

   private void OnNormalTip(object sender, EventArgs e)
   {
      tipPercentSlider.Value = 15;
   }

   private void OnGenerousTip(object sender, EventArgs e)
   {
      tipPercentSlider.Value = 20;
   }
}