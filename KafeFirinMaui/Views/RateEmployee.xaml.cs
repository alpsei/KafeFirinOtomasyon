using KafeFirinMaui.ViewModels;
using SharedClass.Classes;
using System.Collections.ObjectModel;

namespace KafeFirinMaui.Views;

public partial class RateEmployee : ContentPage
{
    private readonly EmployeeRatePageViewModel _viewModel;
    public RateEmployee(EmployeeRatePageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        _ = LoadSelectedEmployeeRatesAsync(_viewModel.RateVM.SelectedEmployeeId);
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.EmployeeVM.LoadEmployeeAsync();
    }
    private void EmployeeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            var selectedEmployee = e.CurrentSelection[0] as Users;

            if (selectedEmployee != null)
            {
                DetailEmployeeID.Text = $"ID: {selectedEmployee.UserID}";
                DetailEmployeeFirstName.Text = $"Ad : {selectedEmployee.FirstName}";
                DetailEmployeeLastName.Text = $"Soyad: {selectedEmployee.LastName}";
                _viewModel.RateVM.SelectedEmployeeId = selectedEmployee.UserID;
                _ = LoadSelectedEmployeeRatesAsync(selectedEmployee.UserID);

                DetailPanel.IsVisible = true;
            }
        }
    }

    private async Task LoadSelectedEmployeeRatesAsync(int employeeId)
    {
        await _viewModel.RateVM.LoadRatesAsync(employeeId);

        if (_viewModel.RateVM.Rates != null && _viewModel.RateVM.Rates.Count > 0)
        {
            double avg = _viewModel.RateVM.Rates.Average(r => r.Rate);
            DetailEmployeeAvgRate.Text = $"Ortalama Puan: {avg:0.0} ★";
        }
        else
        {
            DetailEmployeeAvgRate.Text = "Henüz puan yok.";
        }
    }


}