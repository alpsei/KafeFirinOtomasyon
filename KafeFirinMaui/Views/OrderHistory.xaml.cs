using KafeFirinMaui.ViewModels;
using SharedClass.Classes;

namespace KafeFirinMaui.Views;

public partial class OrderHistory : ContentPage
{
	private readonly OrderHistoryViewModel _viewModel;
    public OrderHistory(OrderHistoryViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadOrderHistoryAsync();
    }
    private void HistoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            var selectedHistory= e.CurrentSelection[0] as Orders;

            if (selectedHistory != null)
            {
                DetailOrderDate.Text = $"Sipari� Tarihi: {selectedHistory.OrderDate.ToString("dd/MM/yyyy")}";
                DetailOrderStatus.Text = $"Sipari� Durumu: {selectedHistory.OrderStatus}";
                DetailOrderNote.Text = $"Sipari� Notu: {selectedHistory.OrderNote}";
                DetailPanel.IsVisible = true;
            }
        }
    }
}