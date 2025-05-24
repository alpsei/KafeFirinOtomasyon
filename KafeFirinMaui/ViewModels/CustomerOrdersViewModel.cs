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
        private readonly FavoriteService _favoriteService;
        public ObservableCollection<ProductViewModel> ProductList { get; set; } = new();
        public ObservableCollection<CartDisplayItem> CartItems { get; set; } = new();
        public Dictionary<Products, int> Cart { get; set; } = new Dictionary<Products, int>();
        public ObservableCollection<int> FavoriteProductsId { get; set; } = new();
        private static readonly decimal[] DiscountRates = { 0.05m, 0.10m, 0.15m };
        public decimal DisplayedTotalPrice => DiscountApplied ? DiscountedTotalPrice : TotalPrice;
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
        private static decimal _currentDiscountRate = 0m;
        public static decimal CurrentDiscountRate
        {
            get => _currentDiscountRate;
            set
            {
                if (_currentDiscountRate != value)
                {
                    _currentDiscountRate = value;
                }
            }
        }
        public int Quantity { get; set; } = 1;
        public bool DiscountApplied { get; set; } = false;

        public ICommand AddToCartCommand { get; }
        public ICommand OrderCommand { get; }


        public CustomerOrdersViewModel(ProductServices productServices, OrderService orderService, FavoriteService favoriteService)
        {
            _productServices = productServices;
            _orderService = orderService;
            _favoriteService = favoriteService;
            AddToCartCommand = new Command<Products>(AddToCart);
            OrderCommand = new Command(OnOrder);
        }

        public async Task LoadProductsAsync()
        {
            var products = await _productServices.GetProductsAsync();
            await LoadFavoritesAsync();

            ProductList.Clear();
            foreach (var product in products)
            {
                ProductList.Add(new ProductViewModel(_productServices, _favoriteService)
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Stock = product.Stock,
                    CategoryID = product.CategoryID,
                    IsFavorite = FavoriteProductsId.Contains(product.ProductID)
                });
            }
        }
        public async Task LoadFavoritesAsync()
        {
            var favorites = await _favoriteService.GetFavoritesAsync();
            FavoriteProductsId.Clear();
            foreach (var fav in favorites.Where(f=> f.CustomerID == Session.LoggedInUser.UserID))
                FavoriteProductsId.Add(fav.ProductID);
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
        public void ResortProductList()
        {
            var sorted = ProductList.OrderByDescending(p => p.IsFavorite).ToList();
            ProductList.Clear();
            foreach (var item in sorted)
                ProductList.Add(item);
        }
        public async void AddToCart(Products product)
        {

            if (Cart.ContainsKey(product))
                Cart[product]++;
            else
                Cart[product] = 1;

            UpdateCartItems();

            await CheckAndApplyDiscountAsync();
        }


        public async Task CheckAndApplyDiscountAsync()
        {
            int orderCount = await _orderService.GetUserOrderCountAsync(Session.LoggedInUser.UserID);
            int newOrderNumber = orderCount + (Cart.Count > 0 ? 1 : 0);

            if (newOrderNumber % 5 == 0 && Cart.Count > 0 && !DiscountApplied)
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
            else if (!(newOrderNumber % 5 == 0 && Cart.Count > 0))
            {
                CurrentDiscountRate = 0;
                DiscountApplied = false;
                foreach (var item in CartItems)
                {
                    item.DiscountRate = 0;
                    item.OnPropertyChanged(nameof(item.DiscountedPrice));
                    item.OnPropertyChanged(nameof(item.DiscountRate));
                    item.OnPropertyChanged(nameof(item.IsDiscounted));
                    OnPropertyChanged(nameof(DisplayedTotalPrice));
                }
            }
        }



        private void UpdateCartItems()
        {
            CartItems.Clear();

            decimal currentDiscountRate = DiscountApplied ? CurrentDiscountRate : 0m;

            foreach (var entry in Cart)
            {
                var product = entry.Key;
                var quantity = entry.Value;
                var originalPrice = product.Price;
                var discountedPrice = originalPrice * (1 - currentDiscountRate);

                CartItems.Add(new CartDisplayItem
                {
                    Product = product,
                    Quantity = quantity,
                    DiscountRate = currentDiscountRate,
                });
            }

            OnPropertyChanged(nameof(CartItems));
            OnPropertyChanged(nameof(TotalPrice));
            OnPropertyChanged(nameof(DiscountedTotalPrice));
            OnPropertyChanged(nameof(DiscountAmount));
            OnPropertyChanged(nameof(DisplayedTotalPrice));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
