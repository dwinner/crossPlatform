using System.Collections.ObjectModel;

namespace c8_DebugVsRelease;

public partial class TestPage
{
   public TestPage()
   {
      InitializeComponent();

      var items = new ObservableCollection<Item>();
      for (var i = 1; i < 30; i++)
      {
         items.Add(new Item(i, $"Item{i}"));
      }

      collectionView.ItemsSource = items;
   }
}