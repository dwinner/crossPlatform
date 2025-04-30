using SwiperApp.Core.Controls;

namespace SwiperApp.Core;

public partial class MainPage
{
   private int _denyCount;
   private int _likeCount;

   public MainPage()
   {
      InitializeComponent();
      AddInitialPhotos();
   }

   private void AddInitialPhotos()
   {
      for (var i = 0; i < 10; i++)
      {
         InsertPhoto();
      }
   }

   private void InsertPhoto()
   {
      var photo = new SwiperControl();
      photo.OnDeny += Handle_OnDeny;
      photo.OnLike += Handle_OnLike;

      mainGrid.Children.Insert(0, photo);
   }

   private void UpdateGui()
   {
      likeLabel.Text = _likeCount.ToString();
      denyLabel.Text = _denyCount.ToString();
   }

   private void Handle_OnLike(object? sender, EventArgs e)
   {
      _likeCount++;
      InsertPhoto();
      UpdateGui();
   }

   private void Handle_OnDeny(object? sender, EventArgs e)
   {
      _denyCount++;
      InsertPhoto();
      UpdateGui();
   }
}