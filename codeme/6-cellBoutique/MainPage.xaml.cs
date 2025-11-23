using CellBoutique.Models;
using System.Collections.ObjectModel;

namespace CellBoutique;

public partial class MainPage : ContentPage
{
    public ObservableCollection<Product> Products { get; set; } = new();

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        Products.Add(new Product
        {
            Title = "Gigaphone XT16",
            Description = "The latest model from Gigaphone features an upgraded camera, more memory and our new AX50 GPU.",
            Price = 1199,
            Image = "https://tinyurl.com/ykyekjdt"
        });

        Products.Add(new Product
        {
            Title = "Gigaphone XT12",
            Description = "The 'little brother'. Has all the same hardware as the flagship XT16 but with a smaller screen.",
            Price = 1099,
            Image = "https://tinyurl.com/bdz2ycfu"
        });

        Products.Add(new Product
        {
            Title = "Gigaphone AF10",
            Description = "This budget friendly model won't break the bank, but doesn't skimp on features either.",
            Price = 999,
            Image = "https://tinyurl.com/4xy8tz8c"
        });

        Products.Add(new Product
        {
            Title = "Gigaphone AF05",
            Description = "Our entry level Gigaphone. It looks cool, but doesn't make calls or access the internet. It does play cool jingles though!",
            Price = 699,
            Image = "https://tinyurl.com/bu9aj25z"
        });
    }
}

