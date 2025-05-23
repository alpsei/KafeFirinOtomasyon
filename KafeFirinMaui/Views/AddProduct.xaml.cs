using KafeFirinMaui.ViewModels;

namespace KafeFirinMaui.Views;

public partial class AddProduct : ContentPage
{
	private readonly ProductViewModel _productViewModel;
    public AddProduct(ProductViewModel viewModel)
	{
		InitializeComponent();
		_productViewModel = viewModel;
        BindingContext = _productViewModel;
    }
    private async void OnProductAddClicked(object sender, EventArgs e)
    {

    }
}