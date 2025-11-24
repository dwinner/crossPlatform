using StockTake.Shared.Products;

namespace StockTake.Mobile.UI.Pages;

[QueryProperty(nameof(Product), nameof(Product))]
public partial class ProductPage : ContentPage
{
   private string _manufacturerName;

   private ProductDto _product;

   private string _productName;

   public ProductPage()
   {
      InitializeComponent();
      BindingContext = this;
   }

   public ProductDto Product
   {
      get => _product;
      set
      {
         _product = value;
         ProductName = _product.Name;
         ManufacturerName = _product.ManufacturerName;
      }
   }

   public string ProductName
   {
      get => _productName;
      set
      {
         _productName = value;
         OnPropertyChanged();
      }
   }

   public string ManufacturerName
   {
      get => _manufacturerName;
      set
      {
         _manufacturerName = value;
         OnPropertyChanged();
      }
   }
}