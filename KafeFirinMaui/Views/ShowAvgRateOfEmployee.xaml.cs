using KafeFirinMaui.Helpers;
using KafeFirinMaui.Services.Interfaces;
using KafeFirinMaui.ViewModels;

namespace KafeFirinMaui.Views;

public partial class ShowAvgRateOfEmployee : ContentPage
{
    private readonly EmployeeRatePageViewModel _viewModel;
    private readonly IUserService _userService;
    public ShowAvgRateOfEmployee(EmployeeRatePageViewModel viewModel, IUserService userService)
	{
		InitializeComponent();
        _viewModel = viewModel;
        _userService = userService;
        BindingContext = _viewModel;
        _ = LoadSelectedEmployeeRatesAsync(_viewModel.RateVM.SelectedEmployeeId);
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.EmployeeVM.LoadEmployeeAsync();
    }
    private async Task LoadSelectedEmployeeRatesAsync(int employeeId)
    {
        employeeId = Session.LoggedInUser?.UserID ?? 0;

        var allUsers = await _userService.GetUsersAsync();
        var currentEmployee = allUsers.FirstOrDefault(x => x.UserID == employeeId);

        if (currentEmployee != null)
        {
            EmployeeNameLabel.Text = $"{currentEmployee.FirstName} {currentEmployee.LastName}";
        }
        else
        {
            EmployeeNameLabel.Text = "Kullanıcı bulunamadı";
        }
        await _viewModel.RateVM.LoadRatesAsync(employeeId);

        if (_viewModel.RateVM.Rates != null && _viewModel.RateVM.Rates.Count > 0)
        {
            double avg = _viewModel.RateVM.Rates.Average(r => r.Rate);
            if(avg == 5)
            {
                EmployeeSuggestionWordLabel.Text = "Mükemmel! Böyle devam et!";
            }
            else if(avg > 3 && avg < 5)
            {
                EmployeeSuggestionWordLabel.Text = "İyi! Biraz daha gayret göstermeye çalış!";
            }
            else
            {
                EmployeeSuggestionWordLabel.Text = "Lütfen potansiyelinin farkında ol!";
            }
                EmployeeAvgRateLabel.Text = $"Ortalama Puanınız: {avg:0.0} ★";
        }
        else
        {
            EmployeeAvgRateLabel.Text = "Henüz puan yok.";
        }
    }
}