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

}