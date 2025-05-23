using KafeFirinMaui.Helpers;
using KafeFirinMaui.Services;
using Windows.System;
using SharedClass.Classes;

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
        try
        {
            if (!string.IsNullOrWhiteSpace(usernameEntry.Text) && !string.IsNullOrWhiteSpace(passwordEntry.Text))
            {
                var user = await userService.UserLogin(usernameEntry.Text, passwordEntry.Text);
                int rolId = user.RoleID;
                if (user == null)
                {
                    await DisplayAlert("Hata", "Kullan�c� ad� veya �ifre hatal�.", "Tamam");
                    return;
                }
                else 
                {
                        
                    DisplayAlert("Bilgi", "Kullan�c� giri�i ba�ar�l�!", "Tamam");
                    usernameEntry.Text = string.Empty;
                    passwordEntry.Text = string.Empty;
                }
                if (Session.LoggedInUser == null)
                {
                    Session.LoggedInUser = new Users();
                }
                Session.LoggedInUser.UserID = user.UserID;
                Session.LoggedInUser.RoleID = rolId;
                Console.WriteLine("Giri� yapan ID: " + Session.LoggedInUser.UserID);
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
                        await DisplayAlert("Hata", "Kullan�c� rol� tan�ml� de�il.", "Tamam");
                        break;
                }
            }
            else
            {
                DisplayAlert("Hata", "Kullan�c� ad� veya �ifre bo� olamaz.", "Tamam");
            }
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Hata", $"Giri� yap�lamad�: {ex.Message}", "Tamam");
        }
    }

    private async void OnRegisterLabelTapped(object sender, EventArgs e)
    {
        try
        {
            var customerRegister = App.Services.GetService<CustomerRegister>();
            if (customerRegister != null)
                await Navigation.PushAsync(customerRegister);

        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Hata", $"Sayfaya ge�i� yap�lamad�: {ex.Message}", "Tamam");
        }

    }
    private async void OnForgotPasswordLabelTapped(object sender, EventArgs e)
    {
        try
        {
            var forgotYourPassword = App.Services.GetService<ForgotYourPassword>();
            if (forgotYourPassword != null)
                await Navigation.PushAsync(forgotYourPassword);

        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Hata", $"Sayfaya ge�i� yap�lamad�: {ex.Message}", "Tamam");
        }

    }

    private void showPasswordCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            passwordEntry.IsPassword = false;
        }
        else
        {
            passwordEntry.IsPassword = true;
        }
    }
}