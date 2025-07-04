namespace Astronomy.Data;

public interface ILatLongService
{
   Task<(double Latitude, double Longitude)> GetLatLong();
}