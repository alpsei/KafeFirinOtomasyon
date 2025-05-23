using KafeFirinMaui.Helpers;
using KafeFirinMaui.ViewModels;

namespace KafeFirinMaui.Views;

public partial class SalaryManagement : ContentPage
{
    private readonly EmployeeViewModel _viewModel;
    public SalaryManagement(EmployeeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
    private void EmployeeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            var selectedEmployee = e.CurrentSelection[0] as Users;
            if (selectedEmployee != null)
            {
                _viewModel.Employee = selectedEmployee;
                DetailPanel.IsVisible = true;
            }
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadEmployeeAsync();
    }

    private async void updateSalaryButton_Clicked(object sender, EventArgs e)
    {
        var result = await _viewModel.UpdateEmployeeInfoAsync();
        await _viewModel.LoadEmployeeAsync();
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