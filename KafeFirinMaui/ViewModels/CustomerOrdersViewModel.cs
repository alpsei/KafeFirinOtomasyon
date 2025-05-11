using KafeFirinMaui.Helpers;
using KafeFirinMaui.Services;
using SharedClass.Classes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KafeFirinMaui.ViewModels
{
    public class CustomerOrdersViewModel : INotifyPropertyChanged
    {
        private readonly ProductServices _productServices;
        private readonly OrderService _orderService;
        public ObservableCollection<Products> ProductList { get; set; } = new();
        public ObservableCollection<Products> Cart { get; set; } = new();

        public int CustomerID { get; set; }
        public int StaffID { get; set; }
        public string OrderNote { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;

        public ICommand AddToCartCommand { get; }
        public ICommand OrderCommand { get; }

        public CustomerOrdersViewModel(ProductServices productServices, OrderService orderService)
        {
            _productServices = productServices;
            _orderService = orderService;
            AddToCartCommand = new Command<Products>(AddToCart);
            OrderCommand = new Command(OnOrder);
        }

        public async Task LoadProductsAsync()
        {
            var products = await _productServices.GetProductsAsync();
            ProductList.Clear();
            foreach (var product in products)
                ProductList.Add(product);
        }

        private async void OnOrder()
        {
            var orderRequest = new OrderRequest
            {
                Order = new Orders
                {
                    CustomerID = Session.LoggedInUser.UserID, 
                    StaffID = 2,   
                    OrderStatus = "Hazırlanıyor",
                    OrderNote = this.OrderNote
                },
                OrderDetails = new List<OrderDetails>()
            };

            foreach (var product in Cart)
            {
                orderRequest.OrderDetails.Add(new OrderDetails
                {
                    ProductID = product.ProductID,
                    Quantity = this.Quantity
                });
            }
            bool success = await _orderService.PlaceOrder(orderRequest);

            if (success)
            {
                Cart.Clear(); 
                await Application.Current.MainPage.DisplayAlert("Başarılı", "Siparişiniz başarıyla alındı.", "Tamam");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Hata", "Sipariş verilirken bir hata oluştu.", "Tamam");
            }
        }

        private void AddToCart(Products product)
        {
            if (product != null)
            {
                Cart.Add(product);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
