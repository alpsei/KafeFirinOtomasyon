using KafeFirinMaui.Helpers;
using KafeFirinMaui.Services;
using SharedClass.Classes;
using System;
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
        public ObservableCollection<CartDisplayItem> CartItems { get; set; } = new();
        public Dictionary<Products, int> Cart { get; set; } = new Dictionary<Products, int>();
        private static readonly decimal[] DiscountRates = { 0.05m, 0.10m, 0.15m };
        private Random _random = new();
        public decimal TotalPrice
        {
            get
            {
                decimal total = 0;
                foreach (var item in CartItems)
                    total += item.OriginalPrice * item.Quantity;
                return total;
            }
        }

        public decimal DiscountedTotalPrice
        {
            get
            {
                decimal total = 0;
                foreach (var item in CartItems)
                    total += (item.IsDiscounted ? item.DiscountedPrice : item.OriginalPrice) * item.Quantity;
                return total;
            }
        }

        public decimal DiscountAmount => TotalPrice - DiscountedTotalPrice;

        public int CustomerID { get; set; }
        public int StaffID { get; set; }
        private string _orderNote;
        public string OrderNote
        {
            get => _orderNote;
            set
            {
                if (_orderNote != value)
                {
                    _orderNote = value;
                    OnPropertyChanged(nameof(OrderNote));
                }
            }
        }
        private decimal _currentDiscountRate = 0m;
        public decimal CurrentDiscountRate
        {
            get => _currentDiscountRate;
            set
            {
                if (_currentDiscountRate != value)
                {
                    _currentDiscountRate = value;
                    OnPropertyChanged();
                    UpdateCartItems();
                }
            }
        }
        public int Quantity { get; set; } = 1;
        public bool DiscountApplied { get; set; } = false;

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
            if (Session.LoggedInUser == null || Session.LoggedInUser.UserID <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Hata", "Lütfen önce giriş yapın.", "Tamam");
                return;
            }
            System.Diagnostics.Debug.WriteLine($"Sipariş veren kullanıcı ID: {Session.LoggedInUser.UserID}");

            int orderCount = await _orderService.GetUserOrderCountAsync(Session.LoggedInUser.UserID);
            int newOrderNumber = orderCount + 1;

            bool discountApplied = false;
            int discountRate = 0;

            if (newOrderNumber % 5 == 0)
            {
                discountApplied = true;
                int[] discountOptions = { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50 };
                Random rnd = new Random();
                discountRate = discountOptions[rnd.Next(discountOptions.Length)];
            }
            DiscountApplied = discountApplied;
            CurrentDiscountRate = discountApplied ? discountRate / 100m : 0m;
            if (Cart.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Hata", "Sepetiniz boş.", "Tamam");
                return;
            }
            var orderRequest = new OrderRequest
            {
                Order = new Orders
                {
                    CustomerID = Session.LoggedInUser.UserID,
                    StaffID = 0,
                    OrderStatus = "Onay Bekliyor",
                    OrderNote = this.OrderNote,
                    DiscountApplied = discountApplied,
                    DiscountRate = discountApplied ? discountRate / 100m : 0m,
                    Customer = null,
                    Staff = null
                },
                OrderDetails = new List<OrderDetails>()
            };

            decimal finalPrice = 0;
            decimal discountedPrice = 0;

            foreach (var entry in Cart)
            {
                var product = entry.Key;
                var quantity = entry.Value;

                decimal lineTotal = product.Price * quantity;
                finalPrice += lineTotal;

                decimal discountedLineTotal = lineTotal;
                if (discountApplied)
                {
                    decimal discountMultiplier = (100 - discountRate) / 100m;
                    discountedLineTotal = lineTotal * discountMultiplier;
                }

                discountedPrice += discountedLineTotal;

                orderRequest.OrderDetails.Add(new OrderDetails
                {
                    ProductID = product.ProductID,
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    TotalAmount = discountedLineTotal,
                    Product = null,
                    Order = null
                });
            }

            bool success = await _orderService.PlaceOrder(orderRequest);

            if (success)
            {
                foreach (var entry in Cart)
                {
                    var product = entry.Key;
                    var quantity = entry.Value;
                    product.Stock -= quantity;
                    await _productServices.UpdateProductAsync(product);
                }

                Cart.Clear();
                DiscountApplied = false;
                CurrentDiscountRate = 0m;
                UpdateCartItems();
                await LoadProductsAsync();
                await Application.Current.MainPage.DisplayAlert("Başarılı", "Siparişiniz başarıyla alındı.", "Tamam");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Hata", "Sipariş verilirken bir hata oluştu.", "Tamam");
            }
        }




        public async void AddToCart(Products product)
        {
            if (product.Stock <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Stok Yok", "Bu ürün stokta yok.", "Tamam");
                return;
            }

            int currentInCart = Cart.ContainsKey(product) ? Cart[product] : 0;

            if (currentInCart + 1 > product.Stock)
            {
                await Application.Current.MainPage.DisplayAlert("Stok Yetersiz", $"Stokta sadece {product.Stock} adet var. Sepete daha fazla ekleyemezsiniz.", "Tamam");
                return;
            }

            if (Cart.ContainsKey(product))
                Cart[product] += 1;
            else
                Cart.Add(product, 1);

            UpdateCartItems();
        }

        public async Task CheckAndApplyDiscountAsync()
        {
            int orderCount = await _orderService.GetUserOrderCountAsync(CustomerID);
            if (orderCount == 5 && !DiscountApplied)
            {
                var rate = DiscountRates[_random.Next(DiscountRates.Length)];
                CurrentDiscountRate = rate;
                DiscountApplied = true;

                foreach (var item in CartItems)
                {
                    item.DiscountRate = rate;
                    item.OnPropertyChanged(nameof(item.DiscountedPrice));
                    item.OnPropertyChanged(nameof(item.DiscountRate));
                    item.OnPropertyChanged(nameof(item.IsDiscounted));
                }
            }
            else if (orderCount < 5)
            {
                CurrentDiscountRate = 0;
                DiscountApplied = false;
                foreach (var item in CartItems)
                {
                    item.DiscountRate = 0;
                    item.OnPropertyChanged(nameof(item.DiscountedPrice));
                    item.OnPropertyChanged(nameof(item.DiscountRate));
                    item.OnPropertyChanged(nameof(item.IsDiscounted));
                }
            }
        }

        private void UpdateCartItems()
        {
            CartItems.Clear();

            decimal currentDiscountRate = DiscountApplied ? CurrentDiscountRate : 0m;

            foreach (var entry in Cart)
            {
                CartItems.Add(new CartDisplayItem
                {
                    Product = entry.Key,
                    Quantity = entry.Value,
                    DiscountRate = currentDiscountRate
                });
            }

            OnPropertyChanged(nameof(CartItems));
            OnPropertyChanged(nameof(TotalPrice));
            OnPropertyChanged(nameof(DiscountedTotalPrice));
            OnPropertyChanged(nameof(DiscountAmount));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
