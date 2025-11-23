namespace StockTake.App.Common.Models;

public class Result
{
   internal Result(bool succeeded, IEnumerable<string> errors)
   {
      Succeeded = succeeded;
      Errors = errors.ToArray();
   }

   public bool Succeeded { get; set; }

   public string[] Errors { get; set; }

   public static Result Success() => new(true, []);

   public static Result Failure(IEnumerable<string> errors) => new(false, errors);
}