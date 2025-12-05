using Android.App;
using Android.Runtime;
using Com.Nostra13.Universalimageloader.Core;

namespace UnoDrive.Droid;

[Application(
   Label = "@string/ApplicationName",
   Icon = "@mipmap/icon",
   LargeHeap = true,
   HardwareAccelerated = true,
   Theme = "@style/Theme.App.Starting"
)]
public class Application : NativeApplication
{
   public Application(IntPtr javaReference, JniHandleOwnership transfer)
      : base(() => new App(), javaReference, transfer)
   {
      ConfigureUniversalImageLoader();
   }

   private static void ConfigureUniversalImageLoader()
   {
      // Create global configuration and initialize ImageLoader with this config
      var config = new ImageLoaderConfiguration.Builder(Context)
         .Build();

      ImageLoader.Instance.Init(config);
   }
}
