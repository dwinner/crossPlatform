namespace Astronomy.Pages;

public partial class AstronomicalBodiesPage
{
   public AstronomicalBodiesPage()
   {
      InitializeComponent();

      btnComet.Clicked += async (_, _) => await Shell.Current.GoToAsync("astronomicalbodydetails?astroName=comet");
      btnEarth.Clicked += async (_, _) => await Shell.Current.GoToAsync("astronomicalbodydetails?astroName=earth");
      btnMoon.Clicked += async (_, _) => await Shell.Current.GoToAsync("astronomicalbodydetails?astroName=moon");
      btnSun.Clicked += async (_, _) => await Shell.Current.GoToAsync("astronomicalbodydetails?astroName=sun");
   }
}