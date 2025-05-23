using KafeFirinMaui.ViewModels;
using KafeFirinMaui.Helpers;
using SharedClass.Classes;
using KafeFirinMaui.Services;

namespace KafeFirinMaui.Views;

public partial class StaffOrders : ContentPage
{
    private StaffOrdersViewModel _viewModel;
    private Orders _selectedOrder;

    public StaffOrders(StaffOrdersViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        LoadOrders();
    }

    private async void LoadOrders()
    {
        int staffId = Session.LoggedInUser.UserID;
        await _viewModel.LoadStaffOrdersAsync(staffId);
    }

    private void HistoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedOrder = e.CurrentSelection.FirstOrDefault() as Orders;
        if (_selectedOrder != null)
        {
            DetailPanel.IsVisible = true;
            DetailOrderDate.Text = $"Tarih: {_selectedOrder.OrderDate:dd/MM/yyyy}";
            DetailOrderStatus.Text = $"Durum: {_selectedOrder.OrderStatus}";
            DetailOrderNote.Text = $"Not: {_selectedOrder.OrderNote}";

            switch (_selectedOrder.OrderStatus)
            {
                case "Hazırlanıyor":
                    updatePreparingButton.IsEnabled = false;
                    updateDeliveredButton.IsEnabled = true;
                    break;
                case "Teslim Edildi":
                    updatePreparingButton.IsEnabled = false;
                    updateDeliveredButton.IsEnabled = false;
                    break;
                default:
                    updatePreparingButton.IsEnabled = true;
                    updateDeliveredButton.IsEnabled = false;
                    break;
            }
        }
        else
        {
            DetailPanel.IsVisible = false;
            updatePreparingButton.IsEnabled = false;
            updateDeliveredButton.IsEnabled = false;
        }
    }


    private async void updatePreparingButton_Clicked(object sender, EventArgs e)
    {
        if (_selectedOrder != null)
        {
            int selectedOrderId = _selectedOrder.OrderID;
            _viewModel.Orders = _selectedOrder;
            _viewModel.Orders.OrderStatus = "Hazırlanıyor";
            var success = await _viewModel.UpdateOrderStatusAsync();

            if (success)
            {
                await _viewModel.LoadStaffOrdersAsync(Session.LoggedInUser.UserID);
                _selectedOrder = _viewModel.StaffOrders.FirstOrDefault(o => o.OrderID == selectedOrderId);
                DetailOrderStatus.Text = $"Durum: {_selectedOrder?.OrderStatus ?? "Bilinmiyor"}";
                await DisplayAlert("Bilgi", "Sipariş durumu 'Hazırlanıyor' olarak güncellendi.", "Tamam");
                updateDeliveredButton.IsEnabled = true;
                updatePreparingButton.IsEnabled = false;
            }
            else
            {
                await DisplayAlert("Hata", "Sipariş durumu güncellenemedi.", "Tamam");
            }
        }
    }

    private async void updateDeliveredButton_Clicked(object sender, EventArgs e)
    {
        if (_selectedOrder != null)
        {
            int selectedOrderId = _selectedOrder.OrderID;
            _viewModel.Orders = _selectedOrder;
            _viewModel.Orders.OrderStatus = "Teslim Edildi";
            var success = await _viewModel.UpdateOrderStatusAsync();

            if (success)
            {
                await _viewModel.LoadStaffOrdersAsync(Session.LoggedInUser.UserID);
                _selectedOrder = _viewModel.StaffOrders.FirstOrDefault(o => o.OrderID == selectedOrderId);
                DetailOrderStatus.Text = $"Durum: {_selectedOrder?.OrderStatus ?? "Bilinmiyor"}";
                await DisplayAlert("Bilgi", "Sipariş durumu 'Teslim Edildi' olarak güncellendi.", "Tamam");
                updateDeliveredButton.IsEnabled = false;
            }
            else
            {
                await DisplayAlert("Hata", "Sipariş durumu güncellenemedi.", "Tamam");
            }
        }
    }


}
