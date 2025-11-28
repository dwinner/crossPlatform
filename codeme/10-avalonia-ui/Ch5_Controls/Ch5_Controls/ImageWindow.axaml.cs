using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System.IO;
using System.Threading.Tasks.Sources;

namespace Ch5_Controls
{
    public partial class ImageWindow : Window
    {
        public ImageWindow()
        {
            InitializeComponent();

            //var image = new Image();
            //var bmp = new Bitmap("Assets/xamarin-forms-succinctly.png");
            //image.Source = bmp;
            //// Stack1 is a StackPanel declared in XAML
            //Stack1.Children.Add(image);
        }
    }
}
