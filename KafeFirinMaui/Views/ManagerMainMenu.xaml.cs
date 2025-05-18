namespace KafeFirinMaui.Views;

public partial class ManagerMainMenu : ContentPage
{
    public ManagerMainMenu()
    {
        InitializeComponent();
    }

    private async void OnStaffButtonTapped(object sender, EventArgs e)
    {
        var addOrRemove = App.Services.GetService<EmployeeAddOrRemove>();
        if (addOrRemove != null)
            await Navigation.PushAsync(addOrRemove);
    }

    private async void OnSalaryButtonTapped(object sender, EventArgs e)
    {
        var salary = App.Services.GetService<SalaryManagement>();
        if (salary != null)
            await Navigation.PushAsync(salary);
    }

    // Bilgilerimi düzenle butonu fonksiyonu
    private async void OnSettingsButtonTapped(object sender, EventArgs e)
    {
        var settings = App.Services.GetService<ManagerSettings>();
        if (settings != null)
            await Navigation.PushAsync(settings);
    }

    // Personel puanlarý butonu fonksiyonu
    private async void OnEmployeeRatesButtonTapped(object sender, EventArgs e)
    {
        var rateEmployee = App.Services.GetService<RateEmployee>();
        if (rateEmployee != null)
            await Navigation.PushAsync(rateEmployee);
    }

    // Geri bildirim gelen kutusu butonu fonksiyonu
    private async void OnFeedbackButtonTapped(object sender, EventArgs e)
    {
        // await Shell.Current.GoToAsync("//StaffPage");
    }

    // Stok takibi butonu fonksiyonu
    private async void OnStockButtonTapped(object sender, EventArgs e)
    {
        var productStock = App.Services.GetService<ProductStock>();
        if (productStock != null)
            await Navigation.PushAsync(productStock);
    }
    private async void OnLogoutButtonTapped(object sender, EventArgs e)
    {
        DisplayAlert("Çýkýþ Yap", "Çýkýþ yaptýnýz.", "Tamam");
        await Shell.Current.Navigation.PopToRootAsync();
        await Shell.Current.GoToAsync("//UserLogin");
    }

}