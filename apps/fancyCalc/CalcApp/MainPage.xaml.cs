namespace CalcApp;

public partial class MainPage
{
   private readonly string[] _numbers = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "."];

   private readonly string[] _operators = ["+", "-", "/", "X", "="];

   private bool _resetOnNextInput;

   private string _selectedOperator;

   public MainPage()
   {
      InitializeComponent();
   }

   public string CurrentInput { get; set; } = string.Empty;

   public string RunningTotal { get; set; } = string.Empty;

   private void Button_Clicked(object sender, EventArgs e)
   {
      var btn = sender as Button;

      var thisInput = btn.Text;

      if (_numbers.Contains(thisInput))
      {
         if (_resetOnNextInput)
         {
            CurrentInput = btn.Text;
            _resetOnNextInput = false;
         }
         else
         {
            CurrentInput += btn.Text;
         }

         lcd.Text = CurrentInput;
      }
      else if (_operators.Contains(thisInput))
      {
         var result = PerformCalculation();

         if (thisInput == "=")
         {
            CurrentInput = result.ToString();

            lcd.Text = CurrentInput;

            RunningTotal = string.Empty;
            _selectedOperator = string.Empty;

            _resetOnNextInput = true;
         }
         else
         {
            RunningTotal = result.ToString();

            _selectedOperator = thisInput;

            CurrentInput = string.Empty;

            lcd.Text = CurrentInput;
         }
      }
   }


   private double PerformCalculation()
   {
      double.TryParse(CurrentInput, out var currentVal);
      double runningVal;
      double.TryParse(RunningTotal, out runningVal);

      double result = _selectedOperator switch
      {
         "+" => runningVal + currentVal,
         "-" => runningVal - currentVal,
         "X" => runningVal * currentVal,
         "/" => runningVal / currentVal,
         _ => currentVal
      };

      return result;
   }
}