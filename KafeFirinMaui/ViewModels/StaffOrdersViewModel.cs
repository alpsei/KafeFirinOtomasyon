using KafeFirinMaui.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using SharedClass.Classes;
using KafeFirinMaui.Helpers;

namespace KafeFirinMaui.ViewModels
{
    public class StaffOrdersViewModel : INotifyPropertyChanged
    {
        private readonly OrderService _orderService;
        public ObservableCollection<Orders> StaffOrders { get; set; } = new();
        private Orders _orders;
        public Orders Orders
        {
            get => _orders;
            set
            {
                if (_orders != value)
                {
                    _orders = value;
                    OnPropertyChanged(nameof(Orders));
                }
            }
        }

        public StaffOrdersViewModel(OrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task LoadStaffOrdersAsync(int staffId)
        {
            try
            {
                var allOrders = await _orderService.GetOrdersAsync();
                var currentUserId = Session.LoggedInUser.UserID;
                var userOrders = allOrders.Where(x => x.StaffID == currentUserId).ToList();
                StaffOrders.Clear();
                foreach (var order in userOrders)
                {
                    StaffOrders.Add(order);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Siparişler yüklenirken hata: {ex.Message}");
            }
        }
        public async Task<bool> UpdateOrderStatusAsync()
        {
            try
            {
                var response = await _orderService.UpdateOrderStatusAsync(Orders);
                if (response)
                {
                    await LoadStaffOrdersAsync(Session.LoggedInUser.UserID);
                    return true;
                }
                else
                {
                    Console.WriteLine("Sipariş durumu güncellenemedi.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sipariş durumu güncellenirken hata: {ex.Message}");
                return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
