namespace FancyCalc;

public partial class MainPage : ContentPage
{
    public string CurrentInput { get; set; } = String.Empty;

    public string RunningTotal { get; set; } = String.Empty;

    private string selectedOperator;

    string[] operators = { "+", "-", "/", "X", "=" };

    string[] numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "." };

    bool resetOnNextInput = false;

    public MainPage()
    {
        InitializeComponent();
    }

    void Button_Clicked(object sender, EventArgs e)
    {
        var btn = sender as Button;

        var thisInput = btn.Text;

        if (numbers.Contains(thisInput))
        {
            if (resetOnNextInput)
            {
                CurrentInput = btn.Text;
                resetOnNextInput = false;
            }
            else
            {
                CurrentInput += btn.Text;
            }

            LCD.Text = CurrentInput;
        }
        else if (operators.Contains(thisInput))
        {
            var result = performCalculation();

            if (thisInput == "=")
            {
                CurrentInput = result.ToString();

                LCD.Text = CurrentInput;

                RunningTotal = String.Empty;
                selectedOperator = String.Empty;

                resetOnNextInput = true;
            }
            else
            {
                RunningTotal = result.ToString();

                selectedOperator = thisInput;

                CurrentInput = String.Empty;

                LCD.Text = CurrentInput;
            }
        }
    }


    double performCalculation()
    {
        double currentVal;
        double.TryParse(CurrentInput, out currentVal);
        double runningVal;
        double.TryParse(RunningTotal, out runningVal);
        double result;

        switch (selectedOperator)
        {
            case "+":
                result = runningVal + currentVal;
                break;
            case "-":
                result = runningVal - currentVal;
                break;
            case "X":
                result = runningVal * currentVal;
                break;
            case "/":
                result = runningVal / currentVal;
                break;
            default:
                result = currentVal;
                break;

        }

        return result;
    }
}

