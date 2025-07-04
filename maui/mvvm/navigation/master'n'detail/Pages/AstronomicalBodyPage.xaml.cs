using Astronomy.Data;

namespace Astronomy.Pages;

[QueryProperty(nameof(AstroName), "astroName")]
public partial class AstronomicalBodyPage
{
   private string _astroName;

   public AstronomicalBodyPage()
   {
      InitializeComponent();
   }

   public string AstroName
   {
      get => _astroName;
      set
      {
         _astroName = value;
         UpdateAstroBodyUi(_astroName);
      }
   }

   private void UpdateAstroBodyUi(string astroName)
   {
      var body = FindAstroData(astroName);
      Title = body.Name;
      lblIcon.Text = body.EmojiIcon;
      lblName.Text = body.Name;
      lblMass.Text = body.Mass;
      lblCircumference.Text = body.Circumference;
      lblAge.Text = body.Age;
   }

   private static AstronomicalBody FindAstroData(string astronomicalBodyName) =>
      astronomicalBodyName switch
      {
         "comet" => SolarSystemData.HalleysComet,
         "earth" => SolarSystemData.Earth,
         "moon" => SolarSystemData.Moon,
         "sun" => SolarSystemData.Sun,
         _ => throw new ArgumentException(string.Empty, nameof(astronomicalBodyName))
      };
}