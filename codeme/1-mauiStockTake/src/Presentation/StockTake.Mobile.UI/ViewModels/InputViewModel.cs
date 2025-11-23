using System.Collections.ObjectModel;
using System.Windows.Input;
using MauiStockTake.Shared.Products;

namespace MauiStockTake.UI.ViewModels;

public class InputViewModel : BaseViewModel
{
    private readonly IProductService _productService;
    private readonly IInventoryService _inventoryService;

    public ICommand SearchProductsCommand { get; set; }

    public ICommand AddCountCommand { get; set; }

    public ObservableCollection<ProductDto> SearchResults { get; set; } = new();

    private ProductDto _selectedProduct;

    public ProductDto SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            _selectedProduct = value;
            if (value is not null)
            {
                StepperEnabled = true;
            }
            else
            {
                StepperEnabled = false;
            }
            OnPropertyChanged();
        }
    }

    private bool _stepperEnabled;

    public bool StepperEnabled
    {
        get => _stepperEnabled;
        set
        {
            _stepperEnabled = value;
            OnPropertyChanged();
        }
    }

    public string SearchTerm { get; set; }
    

    private int _count = 0;
    public int Count
    {
        get => _count;
        set
        {
            _count = value;
            OnPropertyChanged();
        }
    }

    public InputViewModel(IProductService productService, IInventoryService inventoryService)
    {
        _productService = productService;
        _inventoryService = inventoryService;
        SearchProductsCommand = new Command(async () => await UpdateSearchResults());
        AddCountCommand = new Command(async () => await AddCount());
//        // The IsEnabled property of Stepper cannot be changed at runtime.
//        // See: https://github.com/dotnet/maui/issues/11050
//        // Until the above bug is resolved, the workaround is to
//        // instantiate _selectedProduct so that the trigger never diables
//        // the Stepper.
//        // This is no longer relevant after you add the custom stepper.
//#if ANDROID        
//        //_selectedProduct = new();
//#endif
    }

    private async Task UpdateSearchResults()
    {
        IsLoading = true;

        SearchResults.Clear();

        var results = await _productService.SearchProducts(SearchTerm);
        
        IsLoading = false;
        OnPropertyChanged(nameof(IsLoading));

        results.ForEach(res => SearchResults.Add(res));
    }

    private async Task AddCount()
    {
        IsLoading = true;

        var added = await _inventoryService.AddStockCount(SelectedProduct, Count);

        IsLoading = false;

        if (added)
        {
            await App.Current.MainPage.DisplayAlert("Count added", "The stock count has been added to the inventory", "OK");
            ResetForm();
        }
        else
        {
            await App.Current.MainPage.DisplayAlert("Count not added", "Something went wrong, please try again", "OK");
        }
    }

    private void ResetForm()
    {
        Count = 0;
        SelectedProduct = null;
        SearchResults.Clear();
        SearchTerm = string.Empty;
        OnPropertyChanged(nameof(SearchTerm));
    }
}
