using System.Data;

namespace Calculator.Services;

internal class ComputeService
{
   public string Evaluate(string expression)
   {
      var dataTable = new DataTable();
      var finalResult = dataTable.Compute(expression, string.Empty);
      return finalResult.ToString();
   }
}