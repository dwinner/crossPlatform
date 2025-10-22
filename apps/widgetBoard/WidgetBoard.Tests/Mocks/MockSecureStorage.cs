namespace WidgetBoard.Tests.Mocks;

public class MockSecureStorage : ISecureStorage
{
   private readonly Dictionary<string, string?> _values = new();

   private MockSecureStorage(string key, string value)
   {
      _values.Add(key, value);
   }

   public Task<string?> GetAsync(string key) => Task.FromResult(_values[key]);

   public Task SetAsync(string key, string value)
   {
      _values[key] = value;
      return Task.CompletedTask;
   }

   public bool Remove(string key) => _values.Remove(key);

   public void RemoveAll()
   {
      _values.Clear();
   }

   public static MockSecureStorage ThatContains(string key, string value) => new(key, value);
}