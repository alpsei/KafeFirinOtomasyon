using KafeFirinMaui.Helpers;
using KafeFirinMaui.ViewModels;
using SharedClass.Classes;

namespace KafeFirinMaui.Views;

public partial class UserSettings : ContentPage
{
	private readonly UserSettingsViewModel _viewModel;
	public UserSettings(UserSettingsViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
    protected override async void OnAppearing()
	{
        base.OnAppearing();
        await _viewModel.LoadUserInfoAsync();
    }

    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        var result = await _viewModel.UpdateUserInfoAsync();
        if (result)
        {
            await DisplayAlert("Baþarýlý", "Kullanýcý bilgileri güncellendi.", "Tamam");
        }
        else
        {
            await DisplayAlert("Hata", "Kullanýcý bilgileri güncellenemedi.", "Tamam");
        }
    }
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Uyarý", "Hesabýnýzý silmek istediðinize emin misiniz?", "Evet", "Hayýr");

        if (answer)
        {
            var result = await _viewModel.DeleteUserAsync(Session.LoggedInUser.UserID);
            if (result)
            {
                await DisplayAlert("Baþarýlý", "Kullanýcý bilgileri silindi.", "Tamam");
                await Shell.Current.GoToAsync("///UserLogin");
            }
            else
            {
                await DisplayAlert("Hata", "Kullanýcý bilgileri silinemedi.", "Tamam");
            }
        }
    }

}