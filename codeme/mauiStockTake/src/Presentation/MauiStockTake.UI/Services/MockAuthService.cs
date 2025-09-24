namespace MauiStockTake.UI.Services;
public class MockAuthService : IAuthService
{
    public Task<bool> LoginAsync() => Task.FromResult(true);
}
