using System.Diagnostics;

namespace CrossPlatformCapabilities;

public partial class BrowserEmailSample
{
   public BrowserEmailSample()
   {
      InitializeComponent();
   }

   private async void EmailButton_Clicked(object sender, EventArgs e)
   {
      var message = new EmailMessage
      {
         Subject = "Support request",
         To = ["support@onecompany.com"],
         Cc = ["myboss@mycompany.com"],
         BodyFormat = EmailBodyFormat.PlainText,
         Body = "We have problems with the Internet connection"
      };

      // Add attachments
      //string attachmentPath =
      //    Path.Combine(Environment.GetFolderPath(
      //Environment.SpecialFolder.MyPictures),
      //"myimage.png");
      //EmailAttachment attachment = new EmailAttachment(attachmentPath);

      //message.Attachments = new List<EmailAttachment>();
      //message.Attachments.Add(attachment);

      await Email.ComposeAsync(message).ConfigureAwait(true);
      Debug.WriteLine(nameof(EmailButton_Clicked));
   }

   private async void BrowserButton_Clicked(object sender, EventArgs e)
   {
      await Browser.OpenAsync(
         "https://www.microsoft.com/en-us",
         BrowserLaunchMode.SystemPreferred
      ).ConfigureAwait(true);
      Debug.WriteLine(nameof(BrowserButton_Clicked));
   }
}