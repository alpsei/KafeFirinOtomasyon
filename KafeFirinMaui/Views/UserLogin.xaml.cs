using KafeFirinMaui.Services;
using Windows.System;

namespace KafeFirinMaui.Views;

public partial class UserLogin : ContentPage
{
    private readonly UserService userService;
    public UserLogin(UserService _userService)
    {
        InitializeComponent();
        userService = _userService;
    }


    private async void LoginOnClicked(object sender, EventArgs e)
    {
        if (usernameEntry.Text != null && passwordEntry.Text != null)
        {
            var user = await userService.UserLogin(usernameEntry.Text, passwordEntry.Text);
            DisplayAlert("Bilgi", "Kullanýcý giriþi baþarýlý!", "Tamam");
            int rolId = user.RoleID;
            switch (rolId)
            {
                case 1:
                    await Shell.Current.GoToAsync("///CustomerMainMenu");
                    break;
                case 2:
                    await Shell.Current.GoToAsync("///StaffMainMenu");
                    break;
                case 3:
                    await Shell.Current.GoToAsync("///ManagerMainMenu");
                    break;
                default:
                    await DisplayAlert("Hata", "Kullanýcý rolü tanýmlý deðil.", "Tamam");
                    break;
            }
        }
        else
        {
            DisplayAlert("Hata", "Kullanýcý adý veya þifre boþ olamaz.", "Tamam");
        }
    }

    private async void OnRegisterLabelTapped(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync("///CustomerRegister");
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Hata", $"Sayfaya geçiþ yapýlamadý: {ex.Message}", "Tamam");
        }

    }
}