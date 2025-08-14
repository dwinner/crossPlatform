using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotdogOrNot.ImageClassifier;
using HotdogOrNot.Models;

namespace HotdogOrNot.ViewModels;

public partial class MainViewModel : ObservableObject
{
   private IClassifier _classifier;
   private Task _initTask;

   [ObservableProperty]
   [NotifyCanExecuteChangedFor(nameof(TakePhotoCommand))]
   [NotifyCanExecuteChangedFor(nameof(PickPhotoCommand))]
   private bool _isClassifying;

   private readonly IMediaPicker _mediaPicker = MediaPicker.Default;

   public MainViewModel() => _ = InitAsync();

   public Task InitAsync()
   {
      if (_initTask == null || _initTask.IsFaulted)
      {
         _initTask = InitTask();
      }

      return _initTask;
   }

   private Task InitTask() => Task.Run(async () =>
   {
      // Get model
      await using var modelStream = await FileSystem.OpenAppPackageFileAsync("hotdog-or-not.onnx");
      using var modelMemoryStream = new MemoryStream();
      await modelStream.CopyToAsync(modelMemoryStream);
      var model = modelMemoryStream.ToArray();

      _classifier = new MlNetClassifier(model);
   });

   [RelayCommand(CanExecute = nameof(CanExecuteClassification))]
   private async Task TakePhoto()
   {
      if (!_mediaPicker.IsCaptureSupported)
      {
         return;
      }

      var status = await AppPermissions.CheckAndRequestRequiredPermissionAsync<Permissions.Camera>();
      if (status == PermissionStatus.Granted)
      {
         status = await AppPermissions.CheckAndRequestRequiredPermissionAsync<Permissions.StorageWrite>();
      }

      if (status == PermissionStatus.Granted)
      {
         var photo = await _mediaPicker.CapturePhotoAsync(
            new MediaPickerOptions
            {
               Title = "Hotdog or Not?"
            });
         var imageToClassify = await ConvertPhotoToBytes(photo);
         var result = await RunClassificationAsync(imageToClassify);
         await MainThread.InvokeOnMainThreadAsync(async () => await
            Shell.Current.GoToAsync("Result", new Dictionary<string, object> { { "result", result } })
         );
      }
   }

   [RelayCommand(CanExecute = nameof(CanExecuteClassification))]
   private async Task PickPhoto()
   {
      var status = await AppPermissions.CheckAndRequestRequiredPermissionAsync<Permissions.Photos>();
      if (status == PermissionStatus.Granted)
      {
         var photo = await _mediaPicker.PickPhotoAsync();
         var imageToClassify = await ConvertPhotoToBytes(photo);
         var result = await RunClassificationAsync(imageToClassify);
         await MainThread.InvokeOnMainThreadAsync(async () => await
            Shell.Current.GoToAsync("Result", new Dictionary<string, object> { { "result", result } })
         );
      }
   }

   private bool CanExecuteClassification => !IsClassifying;

   private static async Task<byte[]> ConvertPhotoToBytes(FileResult photo)
   {
      if (photo == null)
      {
         return [];
      }

      await using var stream = await photo.OpenReadAsync();
      using MemoryStream memoryStream = new();
      await stream.CopyToAsync(memoryStream);

      return memoryStream.ToArray();
   }

   private async Task<Result> RunClassificationAsync(byte[] imageToClassify)
   {
      IsClassifying = true;

      try
      {
         await InitAsync().ConfigureAwait(false);
         var result = _classifier.Classify(imageToClassify);
         return new Result
         {
            IsHotdog = result.TopResultLabel == "hotdog",
            Confidence = result.TopResultScore,
            PhotoBytes = result.Image
         };
      }
      catch
      {
         return new Result
         {
            IsHotdog = false,
            Confidence = 0.0f,
            PhotoBytes = imageToClassify
         };
      }
      finally
      {
         MainThread.BeginInvokeOnMainThread(() => IsClassifying = false);
      }
   }
}