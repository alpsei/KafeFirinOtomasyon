namespace KafeFirinMaui.Views;

public partial class ManagerMainMenu : ContentPage
{
	public ManagerMainMenu()
	{
		InitializeComponent();
	}

	private async void OnStaffButtonTapped(object sender, EventArgs e)
    {
       // await Shell.Current.GoToAsync("//StaffPage");
    }
    
    private async void OnSalaryButtonTapped(object sender, EventArgs e)
    {
       // await Shell.Current.GoToAsync("//StaffPage");
    }

    // Bilgilerimi düzenle butonu fonksiyonu
    private async void OnSettingsButtonTapped(object sender, EventArgs e)
    {
       // await Shell.Current.GoToAsync("//StaffPage");
    }
    
    // Personel puanlarý butonu fonksiyonu
    private async void OnEmployeeRatesButtonTapped(object sender, EventArgs e)
    {
       // await Shell.Current.GoToAsync("//StaffPage");
    }
    
    // Geri bildirim gelen kutusu butonu fonksiyonu
    private async void OnFeedbackButtonTapped(object sender, EventArgs e)
    {
       // await Shell.Current.GoToAsync("//StaffPage");
    }
    
    // Stok takibi butonu fonksiyonu
    private async void OnStockButtonTapped(object sender, EventArgs e)
    {
       // await Shell.Current.GoToAsync("//StaffPage");
    }
    private async void OnLogoutButtonTapped(object sender, EventArgs e)
    {
        DisplayAlert("Çýkýþ Yap", "Çýkýþ yaptýnýz.", "Tamam");
        await Shell.Current.Navigation.PopToRootAsync();
        await Shell.Current.GoToAsync("//UserLogin");
    }

}