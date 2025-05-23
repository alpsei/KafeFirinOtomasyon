using KafeFirinMaui.ViewModels;

namespace KafeFirinMaui.Views;

public partial class ManagerSettings : ContentPage
{
    private readonly UserSettingsViewModel _viewModel;
    public ManagerSettings(UserSettingsViewModel viewModel)
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
}