using KafeFirinMaui.Services;
using SharedClass.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Dispatching;
using Windows.System;
using KafeFirinMaui.Helpers;

namespace KafeFirinMaui.ViewModels
{
    public class ProductViewModel : Products, INotifyPropertyChanged
    {
        private readonly ProductServices _productServices;
        private readonly FavoriteService _favoriteService;
        private Products _products;
        private ObservableCollection<Products> _productList { get; set; } = new();
        public ObservableCollection<int> FavoriteProductIds { get; set; } = new();
        public ObservableCollection<Products> ProductList
        {
            get => _productList;
            set
            {
                if (_productList != value)
                {
                    _productList = value;
                    OnPropertyChanged();
                }
            }
        }
        public Products Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }
        private bool _isFavorite;
        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                if (_isFavorite != value)
                {
                    _isFavorite = value;
                    OnPropertyChanged();
                }
            }
        }
        public ProductViewModel(ProductServices productServices, FavoriteService favoriteService)
        {
            _productServices = productServices;
            _favoriteService = favoriteService;
        }
        public async Task LoadFavoritesAsync(int userId)
        {
            var favorites = await _favoriteService.GetFavoritesAsync();
            FavoriteProductIds.Clear();
            foreach (var fav in favorites.Where(f => f.CustomerID == userId))
                FavoriteProductIds.Add(fav.ProductID);
        }
        public async Task LoadProductsAsync()
        {
            try
            {
                await LoadFavoritesAsync(Session.LoggedInUser.UserID);
                var products = await _productServices.GetProductsAsync();
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ProductList.Clear();
                    var sortedProducts = products
                        .OrderByDescending(p => FavoriteProductIds.Contains(p.ProductID))
                        .ToList();
                    foreach (var product in sortedProducts)
                    {
                        ProductList.Add(new ProductViewModel(_productServices, _favoriteService)
                        {
                            ProductID = product.ProductID,
                            ProductName = product.ProductName,
                            Price = product.Price,
                            Stock = product.Stock,
                            CategoryID = product.CategoryID,
                            IsFavorite = FavoriteProductIds.Contains(product.ProductID)
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading products: {ex.Message}");
            }
        }
        public async Task<bool> AddProductAsync(Products product)
        {
            try
            {
                var result = await _productServices.AddProductAsync(product);
                if (result)
                {
                    ProductList.Add(product);
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ürün eklenemedi: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> UpdateProductAsync()
        {
            try
            {
                var result = await _productServices.UpdateProductAsync(Products);
                if (result)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("API güncelleme işlemi başarısız döndü.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ürün bilgileri güncellenirken hata: {ex.Message}");
                return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
