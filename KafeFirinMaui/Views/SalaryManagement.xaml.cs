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
                DetailEmployeeSalary.Text = $"{selectedEmployee.Salary}";

                DetailPanel.IsVisible = true;
            }
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.UpdateEmployeeInfoAsync();
    }
}