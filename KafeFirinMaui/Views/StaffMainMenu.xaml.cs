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

    }
    // Puan ortalama butonu fonksiyonu
    private async void OnPointsButtonTapped(object sender, EventArgs e)
    {

    }
    // Stok butonu fonksiyonu
    private async void OnStocksButtonTapped(object sender, EventArgs e)
    {

    }
    private async void OnLogoutButtonTapped(object sender, EventArgs e)
    {
        DisplayAlert("��k�� Yap", "��k�� yapt�n�z.", "Tamam");
        await Shell.Current.Navigation.PopToRootAsync();
        await Shell.Current.GoToAsync("//UserLogin");
    }
}