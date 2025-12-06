namespace UnoBookRail.Common;

public class DefaultTimeAbstraction : ITimeAbstraction
{
    public DateTimeOffset GetNow() => DateTimeOffset.Now;
}