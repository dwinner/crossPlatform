namespace Gallery.Core.Services;

public interface ILocalStorage
{
   void Store(string aFilename);

   List<string> GetFiles();
}