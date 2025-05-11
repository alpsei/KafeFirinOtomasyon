using KafeFirinMaui.ViewModels;
using SharedClass.Classes;
using System.Collections.ObjectModel;

namespace KafeFirinMaui.Views;

public partial class RateEmployee : ContentPage
{
	private readonly EmployeeViewModel _viewModel;
    public RateEmployee(EmployeeViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadEmployeeAsync();
    }
    private void EmployeeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            var selectedEmployee = e.CurrentSelection[0] as Users;

            if (selectedEmployee != null)
            {
                DetailEmployeeID.Text = $"ID: {selectedEmployee.UserID}";
                DetailEmployeeFullName.Text = $"Ad : {selectedEmployee.FirstName}, Soyad: {selectedEmployee.LastName}";
                
                DetailPanel.IsVisible = true;
            }
        }
    }

}