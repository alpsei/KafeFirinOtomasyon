using ABI.System.Collections.Generic;
using KafeFirinMaui.ViewModels;
using SharedClass.Classes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace KafeFirinMaui.Views;

public partial class CustomerOrders : ContentPage
{
    private readonly CustomerOrdersViewModel _viewModel;
    public CustomerOrders(CustomerOrdersViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadProductsAsync();
    }
    private void OnHeartClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button)
        {
            if (button.ClassId == "empty")
            {
                button.Source = "filled_heart.png";
                button.ClassId = "filled";
            }
            else
            {
                button.Source = "heart.png";
                button.ClassId = "empty";
            }
        }
    }
}