namespace CrossPlatformCapabilities;

internal class FileHelper
{
   public static string ReadData()
   {
      try
      {
         var fileName = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "appdata.txt"
         );
         var text = File.ReadAllText(fileName);

         return text;
      }
      catch (Exception)
      {
         return string.Empty;
      }
   }

   public static bool WriteData(string data)
   {
      try
      {
         var fileName = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "appdata.txt"
         );
         File.WriteAllText(fileName, data);

         return true;
      }
      catch (Exception)
      {
         return false;
      }
   }

   public static async Task<string> ReadDataAsync()
   {
      try
      {
         //var mainDir = FileSystem.AppDataDirectory;
         await using var stream = await FileSystem.OpenAppPackageFileAsync("appdata.txt").ConfigureAwait(true);
         using var reader = new StreamReader(stream);
         var localData = await reader.ReadToEndAsync().ConfigureAwait(true);

         return localData;
      }
      catch (Exception)
      {
         return string.Empty;
      }
   }
}