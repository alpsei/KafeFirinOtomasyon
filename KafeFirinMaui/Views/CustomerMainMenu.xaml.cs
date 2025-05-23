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
        var rateEmployeePage = App.Services.GetService<EmployeeRateList>();
        if (rateEmployeePage != null)
            await Navigation.PushAsync(rateEmployeePage);
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

    private async void OnFeedbackButtonTapped(object sender, EventArgs e)
    {
        var customerFeedback = App.Services.GetService<CustomerFeedback>();
        if (customerFeedback != null)
            await Navigation.PushAsync(customerFeedback);
    }
    private async void OnFeedbackHistoryButtonTapped(object sender, EventArgs e)
    {
        var feedbackHistory = App.Services.GetService<FeedbackHistory>();
        if (feedbackHistory != null)
            await Navigation.PushAsync(feedbackHistory);
    }
    private async void OnLogoutButtonTapped(object sender, EventArgs e)
    {
        DisplayAlert("��k�� Yap", "��k�� yapt�n�z.", "Tamam");
        await Shell.Current.Navigation.PopToRootAsync();
        await Shell.Current.GoToAsync("//UserLogin");
    }
}