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
            await DisplayAlert("Ba�ar�l�", "Kullan�c� bilgileri g�ncellendi.", "Tamam");
        }
        else
        {
            await DisplayAlert("Hata", "Kullan�c� bilgileri g�ncellenemedi.", "Tamam");
        }
    }
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Uyar�", "Hesab�n�z� silmek istedi�inize emin misiniz?", "Evet", "Hay�r");

        if (answer)
        {
            var result = await _viewModel.DeleteUserAsync(Session.LoggedInUser.UserID);
            if (result)
            {
                await DisplayAlert("Ba�ar�l�", "Kullan�c� bilgileri silindi.", "Tamam");
                await Shell.Current.GoToAsync("///UserLogin");
            }
            else
            {
                await DisplayAlert("Hata", "Kullan�c� bilgileri silinemedi.", "Tamam");
            }
        }
    }

}