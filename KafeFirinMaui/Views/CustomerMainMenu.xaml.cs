namespace KafeFirinMaui.Views;

public partial class CustomerMainMenu : ContentPage
{
	public CustomerMainMenu()
	{
		InitializeComponent();
	}

    private async void OnOrderButtonTapped(object sender, EventArgs e)
    {
        var ordersPage = App.Services.GetService<CustomerOrders>();
        if (ordersPage != null)
            await Navigation.PushAsync(ordersPage);
    }
    private async void OnStaffVotingButtonTapped(object sender, EventArgs e)
    {
        var rateEmployee= App.Services.GetService<RateEmployee>();
        if (rateEmployee != null)
            await Navigation.PushAsync(rateEmployee);
    }
    private async void OnHistoryButtonTapped(object sender, EventArgs e)
    {
        var orderHistory = App.Services.GetService<OrderHistory>();
        if (orderHistory != null)
            await Navigation.PushAsync(orderHistory);
    }
    private async void OnSettingsButtonTapped(object sender, EventArgs e)
    {
        var userSettings = App.Services.GetService<UserSettings>();
        if (userSettings != null)
            await Navigation.PushAsync(userSettings);
    }
    private async void OnLogoutButtonTapped(object sender, EventArgs e)
    {
        DisplayAlert("Çýkýþ Yap", "Çýkýþ yaptýnýz.", "Tamam");
        await Shell.Current.Navigation.PopToRootAsync();
        await Shell.Current.GoToAsync("//UserLogin");
    }
} 