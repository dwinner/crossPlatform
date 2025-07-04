using System.Reflection;

namespace WorkingWithDocuments;

public partial class MainPage
{
   public MainPage()
   {
      InitializeComponent();
   }

   private void OpenButton_Clicked(object sender, EventArgs e)
   {
      var fileStream = typeof(App).GetTypeInfo().Assembly
         .GetManifestResourceStream("WorkingWithDocuments.SampleDoc.pdf");
      pdfViewerControl.LoadDocument(fileStream);
   }

   private async void ShareButton_Clicked(object sender, EventArgs e)
   {
      await ShareAsync();
   }

   private async Task ShareAsync()
   {
      var fileStream = typeof(App).GetTypeInfo().Assembly
         .GetManifestResourceStream("WorkingWithDocuments.SampleDoc.pdf");

      var cacheFile = Path.Combine(FileSystem.CacheDirectory, "SampleDoc.pdf");
      await using (var file = new FileStream(cacheFile, FileMode.Create, FileAccess.Write))
      {
         await fileStream.CopyToAsync(file).ConfigureAwait(true);
      }

      var request = new ShareFileRequest
      {
         Title = "Share document",
         File = new ShareFile(cacheFile)
      };
      await Share.RequestAsync(request);
   }
}