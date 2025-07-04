namespace CrossPlatformCapabilities;

public partial class FilePickerPage
{
   public FilePickerPage()
   {
      InitializeComponent();
   }

   private async void PickButton_Clicked(object sender, EventArgs e)
   {
      var result = await PickAndShowAsync(
         new PickOptions
         {
            PickerTitle = "Pick a file"
         }
      ).ConfigureAwait(true);
      imageNameLabel.Text = result.FileName;
      pickedImage.Source = result.Image;
   }

   private static async Task<FileSelection> PickAndShowAsync(PickOptions options)
   {
      try
      {
         var result = await FilePicker.PickAsync(options).ConfigureAwait(true);
         var fileResult = new FileSelection();
         if (result != null)
         {
            fileResult.FileName = $"File Name: {result.FileName}";
            if (result.FileName.EndsWith("jpg",
                   StringComparison.OrdinalIgnoreCase) ||
                result.FileName.EndsWith("png",
                   StringComparison.OrdinalIgnoreCase))
            {
               var stream = await result.OpenReadAsync().ConfigureAwait(true);
               fileResult.Image = ImageSource.FromStream(() => stream);
            }
         }

         return fileResult;
      }
      catch (Exception ex)
      {
         return null;
         // The user canceled or something went wrong
      }
   }
}