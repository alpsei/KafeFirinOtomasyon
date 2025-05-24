using KafeFirinMaui.ViewModels;
using SharedClass.Classes;

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
        var entries = MainGrid.Children.OfType<Entry>().ToList();

        string productName = entries[0].Text?.Trim();
        string priceText = entries[1].Text?.Trim();
        string stockText = entries[2].Text?.Trim();

        string selectedCategory = categoryNamePicker.SelectedItem as string;

        if (string.IsNullOrWhiteSpace(productName) ||
            string.IsNullOrWhiteSpace(priceText) ||
            string.IsNullOrWhiteSpace(stockText) ||
            string.IsNullOrWhiteSpace(selectedCategory))
        {
            await DisplayAlert("Hata", "L�tfen t�m alanlar� doldurun.", "Tamam");
            return;
        }

        if (!decimal.TryParse(priceText, out decimal price) || !int.TryParse(stockText, out int stock))
        {
            await DisplayAlert("Hata", "Fiyat ve stok say�sal olmal�d�r.", "Tamam");
            return;
        }

        int categoryId = selectedCategory switch
        {
            "��ecek" => 1,
            "Unlu Mam�ller" => 2,
            "Kahvalt�" => 3,
            "Tatl�/Pasta" => 4,
            "Sandvi�/Tost" => 5,
            _ => 0
        };

        if (categoryId == 0)
        {
            await DisplayAlert("Hata", "Ge�ersiz kategori se�imi.", "Tamam");
            return;
        }

        var newProduct = new Products
        {
            ProductName = productName,
            Price = price,
            Stock = stock,
            CategoryID = categoryId
        };

        bool result = await _productViewModel.AddProductAsync(newProduct);

        if (result)
        {
            await DisplayAlert("Ba�ar�l�", "�r�n eklendi.", "Tamam");
            entries.ForEach(e => e.Text = string.Empty);
            categoryNamePicker.SelectedIndex = -1;
        }
        else
        {
            await DisplayAlert("Hata", "�r�n eklenemedi.", "Tamam");
        }
    }

}