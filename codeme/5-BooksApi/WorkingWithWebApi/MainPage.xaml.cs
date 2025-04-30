using CommunityToolkit.Mvvm.Messaging;
using WorkingWithWebApi.Messages;
using WorkingWithWebApi.ViewModel;

namespace WorkingWithWebApi
{
    public partial class MainPage : ContentPage
    {
        private BookViewModel ViewModel { get; set; }

        public MainPage()
        {
            InitializeComponent();
            WeakReferenceMessenger.Default.Register<DataStatusMessage>(
            this, (sender, message) =>
            {
                ManageDataStatusChanged(message);
            });
            ViewModel = new BookViewModel();
            BindingContext = ViewModel;
        }

        private async void ManageDataStatusChanged(DataStatusMessage message)
        {
            var value = message.Value;
            switch(value)
            {
                case DataStatus.BookDeleted:
                    await DisplayAlert("Deleted", 
                        "The specified book was deleted", "OK");
                    break;
                case DataStatus.BookSaved:
                    LayoutRoot.BackgroundColor = Colors.White;
                    NewBookGrid.IsVisible = false;
                    break;
                case DataStatus.BookError:
                    await DisplayAlert("Error", 
                        "An error has occurred", "OK");
                    LayoutRoot.BackgroundColor = Colors.White;
                    NewBookGrid.IsVisible = false;
                    break;
                default: 
                    break;
            }
        }

        private void AddBookButton_Clicked(object sender, EventArgs e)
        {
            LayoutRoot.BackgroundColor = Colors.LightGray;
            NewBookGrid.IsVisible = true;
            ViewModel.NewBook = new Model.Book();
        }
    }
}