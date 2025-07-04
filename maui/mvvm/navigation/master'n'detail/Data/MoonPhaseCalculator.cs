namespace Astronomy;

public static class MoonPhaseCalculator
{
   public enum Phase
   {
      New,
      WaxingCrescent,
      FirstQuarter,
      WaxingGibbous,
      Full,
      WaningGibbous,
      LastQuarter,
      WaningCrescent
   }

   private const double SynodicLength = 29.530588853; //length in days of a complete moon cycle

   private static readonly DateTime ReferenceNewMoonDate = new(2017, 11, 18);

   public static Phase GetPhase(DateTime date) => GetPhase(GetAge(date));

   private static double GetAge(DateTime date)
   {
      var days = (date - ReferenceNewMoonDate).TotalDays;
      return days % SynodicLength;
   }

   private static Phase GetPhase(double age) =>
      age switch
      {
         < 1 => Phase.New,
         < 7 => Phase.WaxingCrescent,
         < 8 => Phase.FirstQuarter,
         < 14 => Phase.WaxingGibbous,
         < 15 => Phase.Full,
         < 22 => Phase.WaningGibbous,
         < 23 => Phase.LastQuarter,
         < 29 => Phase.WaningCrescent,
         _ => Phase.New
      };
}