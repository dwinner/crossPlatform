namespace Weather.Models;

public partial class ForecastGroup : List<ForecastItem>
{
   public ForecastGroup()
   {
   }

   public ForecastGroup(IEnumerable<ForecastItem> items)
   {
      AddRange(items);
   }

   public DateTime Date { get; set; }

   public string DateAsString => Date.ToShortDateString();

   public List<ForecastItem> Items => this;
}