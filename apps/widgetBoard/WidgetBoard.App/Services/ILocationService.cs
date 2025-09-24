namespace WidgetBoard.App.Services;

public interface ILocationService
{
   Task<Location?> GetLocationAsync();
}