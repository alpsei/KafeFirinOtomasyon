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

namespace KafeFirinMaui.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private readonly ProductServices _productServices;
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
