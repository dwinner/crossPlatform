using System.Reflection;

namespace WorkingWithDocuments;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

    private void OpenButton_Clicked(object sender, EventArgs e)
    {
        var fileStream = typeof(App).GetTypeInfo().Assembly.
            GetManifestResourceStream("WorkingWithDocuments.SampleDoc.pdf");

        PdfViewerControl.LoadDocument(fileStream);

    }

    private async void ShareButton_Clicked(object sender, EventArgs e)
    {
        await ShareAsync();
    }

    private async Task ShareAsync()
    {
        var fileStream = typeof(App).GetTypeInfo().Assembly.
            GetManifestResourceStream("WorkingWithDocuments.SampleDoc.pdf");

        var cacheFile = Path.Combine(FileSystem.CacheDirectory,
            "SampleDoc.pdf");

        using (var file = new FileStream(cacheFile, FileMode.Create,
                          FileAccess.Write))
        {
            fileStream.CopyTo(file);
        }
        var request = new ShareFileRequest();
        request.Title = "Share document";
        request.File = new ShareFile(cacheFile);
        await Share.RequestAsync(request);
    }

}

