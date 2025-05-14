using System.Collections.ObjectModel;
using Gallery.Core.Models;

namespace Gallery.Core.Services;

public interface IPhotoImporter
{
   Task<string[]> ImportAsync();

   Task<ObservableCollection<Photo>> GetPhotosAsync(int start, int count, Quality quality = Quality.Low);

   Task<ObservableCollection<Photo>> GetPhotosAsync(List<string> filenames, Quality quality = Quality.Low);
}