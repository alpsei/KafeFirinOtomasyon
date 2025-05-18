using KafeFirinMaui.Helpers;
using KafeFirinMaui.ViewModels;
using SharedClass.Classes;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KafeFirinMaui.Views;

public partial class EmployeeRateList : ContentPage
{
    private readonly EmployeeRatePageViewModel _viewModel;
    public EmployeeRateList(EmployeeRatePageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.EmployeeVM.LoadEmployeeAsync();
    }
    public async void EmployeeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            var selectedEmployee = e.CurrentSelection.FirstOrDefault() as Users;

            if (selectedEmployee != null)
            {
                int selectedEmployeeId = selectedEmployee.UserID;
                DetailEmployeeID.Text = $"ID: {selectedEmployee.UserID}";
                DetailEmployeeFullName.Text = $"Ad: {selectedEmployee.FirstName}, Soyad: {selectedEmployee.LastName}";
                DetailPanel.IsVisible = true;

                _viewModel.RateVM.SelectedEmployeeId = selectedEmployee.UserID;
                _viewModel.RateVM.CurrentCustomerId = Session.LoggedInUser.UserID;
                await _viewModel.RateVM.LoadRatesAsync(selectedEmployee.UserID);
                UpdateStars(_viewModel.RateVM.CurrentRating);
            }
        }
    }
    private async void OnSendRateClicked(object sender, EventArgs e)
    {
        var employeeIdText = DetailEmployeeID.Text;
        var employeeId = int.Parse(employeeIdText.Split(":")[1].Trim());
        var rateData = new Rates
        {
            StaffID = employeeId,
            CustomerID = Session.LoggedInUser.UserID,
            Rate = _viewModel.RateVM.CurrentRating
        };
        if (_viewModel.RateVM.SendRateCommand.CanExecute(null))
        {
            await _viewModel.RateVM.SendRateAsync();
            await DisplayAlert("Baþarýlý", "Puan gönderildi.", "Tamam");
            DetailPanel.IsVisible = false;
            EmployeeList.SelectedItem = null;
        }
        else
        {
            await DisplayAlert("Hata", "Lütfen bir çalýþan seçin ve puan verin.", "Tamam");
        }
    }

    private void UpdateStars(int rating)
    {
        foreach (var star in RatingStars.Children.OfType<ImageButton>())
        {
            int starValue = int.Parse(star.CommandParameter.ToString());
            star.Source = starValue <= rating ? "filled_star.png" : "star.png";
        }
    }

    private int currentRating = 0;

    private void OnStarClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton clickedStar && int.TryParse(clickedStar.CommandParameter?.ToString(), out int rating))
        {
            _viewModel.RateVM.CurrentRating = rating;
            UpdateStars(rating);
        }
    }




}