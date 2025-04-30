namespace CrossPlatformCapabilities;

public partial class BrowserEmailSample : ContentPage
{
	public BrowserEmailSample()
	{
		InitializeComponent();
	}

    private async void EmailButton_Clicked(object sender, EventArgs e)
    {
        EmailMessage message = new EmailMessage();
        message.Subject = "Support request";
        message.To = new List<string> { "support@onecompany.com" };
        message.Cc = new List<string> { "myboss@mycompany.com" };
        message.BodyFormat = EmailBodyFormat.PlainText;
        message.Body = "We have problems with the Internet connection";
        
        // Add attachments
        //string attachmentPath =
        //    Path.Combine(Environment.GetFolderPath(
        //Environment.SpecialFolder.MyPictures),
        //"myimage.png");
        //EmailAttachment attachment = new EmailAttachment(attachmentPath);

        //message.Attachments = new List<EmailAttachment>();
        //message.Attachments.Add(attachment);

        await Email.ComposeAsync(message);

    }

    private async void BrowserButton_Clicked(object sender, EventArgs e)
    {
        await Browser.OpenAsync("https://www.microsoft.com/en-us",
              BrowserLaunchMode.SystemPreferred);

    }
}