using System.Net.Http.Headers;
using Microsoft.UI.Xaml.Media.Imaging;
using UnoDrive.Data;

namespace UnoDrive.Services;

public class GraphFileService : IGraphFileService, IAuthenticationProvider
{
   private GraphServiceClient graphClient;
   private readonly IDataStore dataStore;
   private readonly INetworkConnectivityService networkConnectivity;
   private readonly ILogger logger;

   public GraphFileService(
      IDataStore dataStore,
      INetworkConnectivityService networkConnectivity,
      ILogger<GraphFileService> logger)
   {
      this.dataStore = dataStore;
      this.networkConnectivity = networkConnectivity;
      this.logger = logger;

      var httpClient = new HttpClient();
      graphClient = new GraphServiceClient(httpClient);
      graphClient.AuthenticationProvider = this;
   }

   public async Task<IEnumerable<OneDriveItem>> GetRootFilesAsync(
      Action<IEnumerable<OneDriveItem>, bool> cachedCallback = null, CancellationToken cancellationToken = default)
   {
      var rootPathId = dataStore.GetRootId();
      if (networkConnectivity.Connectivity == NetworkConnectivityLevel.InternetAccess)
      {
         try
         {
            var request = graphClient.Me.Drive.Root.Request();

#if __ANDROID__ || __IOS__ || __MACOS__
					var response = await request.GetResponseAsync(cancellationToken);
					var data = await response.Content.ReadAsStringAsync();
					var rootNode = JsonSerializer.Deserialize<DriveItem>(data);
#else
            var rootNode = await request.GetAsync(cancellationToken);
#endif

            if (rootNode == null || string.IsNullOrEmpty(rootNode.Id))
            {
               throw new KeyNotFoundException("Unable to find OneDrive Root Folder");
            }

            rootPathId = rootNode.Id;
            dataStore.SaveRootId(rootPathId);
         }
         catch (TaskCanceledException ex)
         {
            logger.LogWarning(ex, ex.Message);
            throw;
         }
         catch (KeyNotFoundException ex)
         {
            logger.LogWarning(
               "Unable to retrieve data from Graph API, it may not exist or there could be a connection issue");
            logger.LogWarning(ex, ex.Message);
            throw;
         }
         catch (Exception ex)
         {
            logger.LogWarning("Unable to retrieve root OneDrive folder");
            logger.LogWarning(ex, ex.Message);
         }
      }

      return await GetMyFilesAsync(rootPathId, cachedCallback, cancellationToken);
   }

   public Task<IEnumerable<OneDriveItem>> GetMyFilesAsync(string id,
      Action<IEnumerable<OneDriveItem>, bool> cachedCallback = null, CancellationToken cancellationToken = default)
   {
      return GetFilesAsync(GraphRequestType.MyFiles, id, cachedCallback, cancellationToken);
   }

   public Task<IEnumerable<OneDriveItem>> GetRecentFilesAsync(
      Action<IEnumerable<OneDriveItem>, bool> cachedCallback = null, CancellationToken cancellationToken = default)
   {
      return GetFilesAsync(GraphRequestType.Recent, "RECENT-FILES", cachedCallback, cancellationToken);
   }

   public Task<IEnumerable<OneDriveItem>> GetSharedFilesAsync(
      Action<IEnumerable<OneDriveItem>, bool> cachedCallback = null, CancellationToken cancellationToken = default)
   {
      return GetFilesAsync(GraphRequestType.SharedWithMe, "SHARED-FILES", cachedCallback, cancellationToken);
   }

#if __ANDROID__ || __IOS__ || __MACOS__
		async Task<UnoDrive.Models.DriveItem[]>
#else
   private async Task<OneDriveItem[]>
#endif
      ProcessGraphRequestAsync(GraphRequestType requestType, string id,
         Action<IEnumerable<OneDriveItem>, bool> cachedCallback, CancellationToken cancellationToken)
   {
#if __ANDROID__ || __IOS__ || __MACOS__
	UnoDrive.Models.DriveItem[] oneDriveItems = null;
#else
      OneDriveItem[] oneDriveItems = null;
#endif

      if (requestType == GraphRequestType.MyFiles)
      {
         var request = graphClient.Me.Drive
            .Items[id]
            .Children
            .Request()
            .Expand("thumbnails");

#if __ANDROID__ || __IOS__ || __MACOS__
		var response = await request.GetResponseAsync(cancellationToken);
		var data = await response.Content.ReadAsStringAsync();
		var collection = JsonSerializer.Deserialize<UnoDrive.Models.DriveItemCollection>(data);
		oneDriveItems = collection.Value;
#else
         oneDriveItems = (await request.GetAsync(cancellationToken)).ToArray();
#endif
         return oneDriveItems;
      }

      if (requestType == GraphRequestType.Recent)
      {
         var request = graphClient.Me.Drive
            .Recent()
            .Request();

#if __ANDROID__ || __IOS__ || __MACOS__
		var response = @await request.GetResponseAsync(cancellationToken);
		var data = @await response.Content.ReadAsStringAsync();
		var collection = JsonSerializer.Deserialize<UnoDrive.Models.DriveItemCollection>(data);
		oneDriveItems = collection.Value;
#else
         oneDriveItems = (await request.GetAsync(cancellationToken)).ToArray();
#endif
      }
      else if (requestType == GraphRequestType.SharedWithMe)
      {
         var request = graphClient.Me.Drive
            .SharedWithMe()
            .Request();

#if __ANDROID__ || __IOS__ || __MACOS__
		var response = @await request.GetResponseAsync(cancellationToken);
		var data = @await response.Content.ReadAsStringAsync();
		var collection = JsonSerializer.Deserialize<UnoDrive.Models.DriveItemCollection>(data);
		oneDriveItems = collection.Value;
#else
         oneDriveItems = (await request.GetAsync(cancellationToken)).ToArray();
#endif
      }

      return oneDriveItems;
   }

   private async Task<IEnumerable<OneDriveItem>> GetFilesAsync(GraphRequestType requestType, string id,
      Action<IEnumerable<OneDriveItem>, bool> cachedCallback = null, CancellationToken cancellationToken = default)
   {
      if (cachedCallback != null)
      {
         var cachedChildren = dataStore
            .GetCachedFiles(id)
            .OrderByDescending(item => item.Type)
            .ThenBy(item => item.Name);

         cachedCallback(cachedChildren, true);
      }

      logger.LogInformation($"Network Connectivity: {networkConnectivity.Connectivity}");
      if (networkConnectivity.Connectivity != NetworkConnectivityLevel.InternetAccess)
      {
         return default;
      }

      cancellationToken.ThrowIfCancellationRequested();
      var oneDriveItems = await ProcessGraphRequestAsync(requestType, id, cachedCallback, cancellationToken);

      var childrenTable = oneDriveItems
         .Select(driveItem => new OneDriveItem
         {
            Id = driveItem.Id,
            Name = driveItem.Name,
            Path = driveItem.ParentReference.Path,
            PathId = driveItem.ParentReference.Id,
            FileSize = $"{driveItem.Size}",
            Modified = driveItem.LastModifiedDateTime.HasValue
               ? driveItem.LastModifiedDateTime.Value.LocalDateTime
               : DateTime.Now,
            Type = driveItem.Folder != null ? OneDriveItemType.Folder : OneDriveItemType.File
         })
         .OrderByDescending(item => item.Type)
         .ThenBy(item => item.Name)
         .ToDictionary(item => item.Id);

      cancellationToken.ThrowIfCancellationRequested();

      var children = childrenTable.Select(item => item.Value).ToArray();
      if (cachedCallback != null)
      {
         cachedCallback(children, false);
      }

      dataStore.SaveCachedFiles(children, id);
      await StoreThumbnailsAsync(oneDriveItems, childrenTable, cachedCallback, cancellationToken);
      return childrenTable.Select(x => x.Value);
   }

#if __ANDROID__ || __IOS__ || __MACOS__
		async Task StoreThumbnailsAsync(UnoDrive.Models.DriveItem[] oneDriveItems, IDictionary<string, OneDriveItem> childrenTable, Action<IEnumerable<OneDriveItem>, bool> cachedCallback
 = null, CancellationToken cancellationToken = default)
#else
   private async Task StoreThumbnailsAsync(DriveItem[] oneDriveItems, IDictionary<string, OneDriveItem> childrenTable,
      Action<IEnumerable<OneDriveItem>, bool> cachedCallback = null, CancellationToken cancellationToken = default)
#endif
   {
      for (var index = 0; index < oneDriveItems.Length; index++)
      {
         var currentItem = oneDriveItems[index];
         var thumbnails = currentItem.Thumbnails?.FirstOrDefault();
         if (thumbnails == null || !childrenTable.ContainsKey(currentItem.Id))
         {
            continue;
         }

         var url = thumbnails.Medium.Url;

         var httpClient = new HttpClient();
         var thumbnailResponse = await httpClient.GetAsync(url, cancellationToken);
         if (!thumbnailResponse.IsSuccessStatusCode)
         {
            continue;
         }

#if HAS_UNO_SKIA_WPF
				var applicationFolder = Path.Combine(ApplicationData.Current.TemporaryFolder.Path, "UnoDrive");
				var imagesFolder = Path.Combine(applicationFolder, "thumbnails");
#else
         var imagesFolder = Path.Combine(ApplicationData.Current.LocalFolder.Path, "thumbnails");
#endif

         var name = $"{currentItem.Id}.jpeg";
         var localFilePath = Path.Combine(imagesFolder, name);

         try
         {
            if (!Directory.Exists(imagesFolder))
            {
               Directory.CreateDirectory(imagesFolder);
            }

            if (File.Exists(localFilePath))
            {
               File.Delete(localFilePath);
            }


            var bytes = await thumbnailResponse.Content.ReadAsByteArrayAsync();

#if HAS_UNO_SKIA_WPF
					System.IO.File.WriteAllBytes(localFilePath, bytes);
#else
            await File.WriteAllBytesAsync(localFilePath, bytes, cancellationToken);
#endif

            // If thumbnails aren't loading using thed Uri code path, try
            // using the fallback strategy with the MemoryStream
#if __UNO_DRIVE_WINDOWS__ || __ANDROID__ || __IOS__
					var image = new BitmapImage(new Uri(localFilePath));
#else
            var image = new BitmapImage();
            image.SetSource(new MemoryStream(bytes));
#endif

            childrenTable[currentItem.Id].ThumbnailSource = image;

            if (cachedCallback != null)
            {
               var children = childrenTable
                  .Select(item => item.Value)
                  .ToArray();
               cachedCallback(children, false);
            }

            dataStore.UpdateCachedFileById(currentItem.Id, localFilePath);
            cancellationToken.ThrowIfCancellationRequested();
         }
         catch (TaskCanceledException ex)
         {
            logger.LogWarning(ex, ex.Message);
            throw;
         }
         catch (Exception ex)
         {
            logger.LogError(ex, ex.Message);
         }
      }
   }

   Task IAuthenticationProvider.AuthenticateRequestAsync(HttpRequestMessage request)
   {
      var token = ((App)App.Current).AuthenticationResult?.AccessToken;
      if (string.IsNullOrEmpty(token))
      {
         throw new Exception("No Access Token");
      }

      request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
      return Task.CompletedTask;
   }

   public async Task AuthenticateRequestAsync(RequestInformation request,
      Dictionary<string, object>? additionalAuthenticationContext = null,
      CancellationToken cancellationToken = new())
   {
      throw new NotImplementedException();
   }
}
