using Gallery.Core.Models;
using Gallery.Core.ViewModels;

namespace Gallery.Core.Views;

public partial class GalleryView
{
   private readonly GalleryViewModel _viewModel;

   public GalleryView(GalleryViewModel viewModel)
   {
      InitializeComponent();
      BindingContext = _viewModel = viewModel;
      MainThread.InvokeOnMainThreadAsync(viewModel.Initialize);
   }

   private void SelectToolBarItem_Clicked(object? sender, EventArgs e)
   {
      var selected = photos.SelectedItems?.Cast<Photo>().ToList();
      if (selected is null || selected.Count == 0)
      {
         DisplayAlert("No photos", "No photos selected", "OK");
         return;
      }

      _viewModel.AddFavoritesCommand.Execute(selected);
      DisplayAlert("Added", "Selected photos has been added to favorites", "OK");
   }
}