using CommunityToolkit.Mvvm.Messaging;
using WorkingWithWebApi.Messages;
using WorkingWithWebApi.Model;
using WorkingWithWebApi.ViewModel;

namespace WorkingWithWebApi;

public partial class MainPage
{
   public MainPage()
   {
      InitializeComponent();
      WeakReferenceMessenger.Default.Register<DataStatusMessage>(
         this, (_, message) => { ManageDataStatusChanged(message); });
      ViewModel = new BookViewModel();
      BindingContext = ViewModel;
   }

   private BookViewModel ViewModel { get; }

   private async void ManageDataStatusChanged(DataStatusMessage message)
   {
      var value = message.Value;
      switch (value)
      {
         case DataStatus.BookDeleted:
            await DisplayAlert("Deleted", "The specified book was deleted", "OK");
            break;

         case DataStatus.BookSaved:
            layoutRoot.BackgroundColor = Colors.White;
            newBookGrid.IsVisible = false;
            break;

         case DataStatus.BookError:
            await DisplayAlert("Error", "An error has occurred", "OK");
            layoutRoot.BackgroundColor = Colors.White;
            newBookGrid.IsVisible = false;
            break;
      }
   }

   private void AddBookButton_Clicked(object sender, EventArgs e)
   {
      layoutRoot.BackgroundColor = Colors.LightGray;
      newBookGrid.IsVisible = true;
      ViewModel.NewBook = new Book();
   }
}