using System.Collections.ObjectModel;

namespace OutlookClone;

public partial class MainPage
{
   public ObservableCollection<Simpson> Simpsons = [];

   public MainPage()
   {
      InitializeComponent();
      messageCollection.ItemsSource = Simpsons;
   }

   protected override async void OnAppearing()
   {
      loadingIndicator.IsVisible = true;

      base.OnAppearing();
      var simpsons = await GetSimpsonsAsync();
      foreach (var item in simpsons)
      {
         Simpsons.Add(item);
      }

      loadingIndicator.IsVisible = false;
   }

   private async Task<IEnumerable<Simpson>> GetSimpsonsAsync()
   {
      const int capacity = 20;
#if DEBUG
      await Task.Delay(TimeSpan.FromSeconds(1));
#endif

      var simpsons = new List<Simpson>(capacity);
      for (var i = 0; i < capacity; i++)
      {
         var simpson = new Simpson
         {
            Character = $"{nameof(Simpson.Character)} #{i}",
            CharacterDirection = $"Character direction #{i}",
            Image = string.Empty,
            Quote = "quick brown fox jumps over the lazy dog"
         };

         simpsons.Add(simpson);
      }

      return simpsons;
   }
}