namespace UnoDrive.Services;

public interface INetworkConnectivityService
{
   NetworkConnectivityLevel Connectivity { get; }

   event EventHandler NetworkStatusChanged;
}
