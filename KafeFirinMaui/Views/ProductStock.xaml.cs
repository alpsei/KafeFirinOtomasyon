using KafeFirinMaui.ViewModels;
using SharedClass.Classes;

namespace KafeFirinMaui.Views
{
    public partial class ProductStock : ContentPage
    {
        private readonly ProductViewModel _productViewModel;

        public ProductStock(ProductViewModel viewModel)
        {
            InitializeComponent();
            _productViewModel = viewModel;
            BindingContext = _productViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _productViewModel.LoadProductsAsync();
        }
        private void ProductList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedProduct = e.CurrentSelection.FirstOrDefault() as Products;
            if (selectedProduct != null)
            {
                _productViewModel.Products = selectedProduct;
                DetailProductNameLabel.Text = selectedProduct.ProductName;
                DetailStockEntry.Text = selectedProduct.Stock.ToString();
                DetailPriceEntry.Text = selectedProduct.Price.ToString();
                DetailPanel.IsVisible = true;
            }
            else
                DetailPanel.IsVisible = false;
        }
        private async void updateStockButton_Clicked(object sender, EventArgs e)
        {
            if (_productViewModel.Products != null)
            {
                _productViewModel.Products.Stock = int.Parse(DetailStockEntry.Text);
                _productViewModel.Products.Price = decimal.Parse(DetailPriceEntry.Text);
            }
            var result = await _productViewModel.UpdateProductAsync();
            if (result)
            {
                await DisplayAlert("Baþarýlý", "Ürün bilgileri güncellendi.", "Tamam");
                _ = _productViewModel.LoadProductsAsync();
            }
            else
            {
                await DisplayAlert("Hata", "Ürün bilgileri güncellenemedi.", "Tamam");
            }
        }
    }
}
