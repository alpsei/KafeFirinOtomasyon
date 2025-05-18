using KafeFirinMaui.ViewModels;

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
    }
}
