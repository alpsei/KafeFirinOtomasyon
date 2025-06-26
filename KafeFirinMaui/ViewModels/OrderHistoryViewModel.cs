using KafeFirinMaui.Helpers;
using KafeFirinMaui.Services.Classes;
using KafeFirinMaui.Services.Interfaces;
using KafeFirinMaui.Views;
using SharedClass.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeFirinMaui.ViewModels
{
    public class OrderHistoryViewModel : INotifyPropertyChanged
    {
        private readonly IOrderService _orderService;
        public ObservableCollection<Orders> OrderHistories { get; set; } = new();
        public OrderHistoryViewModel(IOrderService orderService)
        {
            _orderService = orderService;
            _ = LoadOrderHistoryAsync();
        }
        public async Task LoadOrderHistoryAsync(DateTime? filterDate = null)
        {
            try
            {
                var allOrders = await _orderService.GetOrdersAsync();
                var currentUserId = Session.LoggedInUser.UserID;
                var userOrders = allOrders.Where(x => x.CustomerID == currentUserId);
                if (filterDate.HasValue)
                {
                    userOrders = userOrders.Where(o => o.OrderDate.Date == filterDate.Value.Date);
                }
                var sortedOrders = userOrders.OrderByDescending(o => o.OrderDate).ToList();

                OrderHistories.Clear();
                foreach (var order in sortedOrders)
                {
                    OrderHistories.Add(order);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sipariş geçmişi yüklenirken hata: {ex.Message}");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
