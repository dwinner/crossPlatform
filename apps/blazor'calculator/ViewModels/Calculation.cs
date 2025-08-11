namespace Calculator.ViewModels;

public class Calculation(string expression, string result) : Tuple<string, string>(expression, result)
{
   public string Expression => Item1;

   public string Result => Item2;
}