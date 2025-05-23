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

namespace KafeFirinMaui.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private readonly ProductServices _productServices;
        private Products _products;
        private ObservableCollection<Products> _productList { get; set; } = new();

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

        public ProductViewModel(ProductServices productServices)
        {
            _productServices = productServices;
        }

        public async Task LoadProductsAsync()
        {
            try
            {
                var products = await _productServices.GetProductsAsync();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ProductList.Clear();
                    foreach (var product in products)
                    {
                        ProductList.Add(product);
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
