using KafeFirinMaui.Helpers;
using KafeFirinMaui.Services;
using KafeFirinMaui.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedClass.Classes;

namespace KafeFirinMaui.ViewModels
{
    public class OrderHistoryViewModel : INotifyPropertyChanged
    {
        private readonly OrderService _orderService;
        public ObservableCollection<Orders> OrderHistories { get; set; } = new();
        public OrderHistoryViewModel(OrderService orderService)
        {
            _orderService = orderService;
            _ = LoadOrderHistoryAsync();
        }

        public async Task LoadOrderHistoryAsync()
        {
            try
            {
                var allOrders = await _orderService.GetOrdersAsync();
                var currentUserId = Session.LoggedInUser.UserID;
                var userOrders = allOrders.Where(x => x.CustomerID == currentUserId).ToList();
                OrderHistories.Clear();
                foreach (var order in userOrders)
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
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
