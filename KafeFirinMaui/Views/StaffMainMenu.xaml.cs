namespace KafeFirinMaui.Views;

public partial class StaffMainMenu : ContentPage
{
    public StaffMainMenu()
    {
        InitializeComponent();
    }
    // Sipari�leri listeleme butonu fonksiyonu
    private async void OnOrderButtonTapped(object sender, EventArgs e)
    {
        var staffOrders = App.Services.GetService<StaffOrders>();
        if (staffOrders != null)
            await Navigation.PushAsync(staffOrders);
    }
    // Puan ortalama butonu fonksiyonu
    private async void OnPointsButtonTapped(object sender, EventArgs e)
    {
        var avgPoint = App.Services.GetService<ShowAvgRateOfEmployee>();
        if (avgPoint != null)
            await Navigation.PushAsync(avgPoint);
    }
    // Stok butonu fonksiyonu
    private async void OnStocksButtonTapped(object sender, EventArgs e)
    {
        var productStock = App.Services.GetService<ProductStock>();
        if (productStock != null)
            await Navigation.PushAsync(productStock);
    }

    // �r�n ekleme butonu
    private async void OnAddProductButtonTapped(object sender, EventArgs e)
    {
        var addProduct = App.Services.GetService<AddProduct>();
        if (addProduct != null)
            await Navigation.PushAsync(addProduct);
    }

    // Bilgilerimi d�zenle butonu
    private async void OnSettingsButtonTapped(object sender, EventArgs e)
    {
        var userSettings = App.Services.GetService<UserSettings>();
        if (userSettings != null)
            await Navigation.PushAsync(userSettings);
    }
    private async void OnLogoutButtonTapped(object sender, EventArgs e)
    {
        DisplayAlert("��k�� Yap", "��k�� yapt�n�z.", "Tamam");
        await Shell.Current.Navigation.PopToRootAsync();
        await Shell.Current.GoToAsync("//UserLogin");
    }
}