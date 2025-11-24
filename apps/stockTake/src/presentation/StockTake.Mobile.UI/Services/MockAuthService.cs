namespace StockTake.Mobile.UI.Services;

public class MockAuthService : IAuthService
{
   public Task<bool> LoginAsync()
   {
      return Task.FromResult(true);
   }
}