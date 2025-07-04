namespace People;

public partial class MainPage
{
   public MainPage()
   {
      InitializeComponent();
   }

   private async void OnNewButtonClicked(object sender, EventArgs args)
   {
      statusMessage.Text = string.Empty;
      await App.PersonRepo.AddAsync(newPerson.Text).ConfigureAwait(true);
      statusMessage.Text = App.PersonRepo.StatusMessage;
   }

   private async void OnGetButtonClicked(object sender, EventArgs args)
   {
      statusMessage.Text = string.Empty;
      var people = await App.PersonRepo.GetAllAsync().ConfigureAwait(true);
      peopleList.ItemsSource = people;
   }
}