using SharedClasses;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KafeFirinMaui.ViewModels
{
    public class CustomerOrdersViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Products> _productList;
        public ObservableCollection<Products> ProductList { get; set; } = new ObservableCollection<Products>();

        public CustomerOrdersViewModel()
        {
            LoadProducts();
        }

        private void LoadProducts()
        {
            // Burada API'den ya da doğrudan DbContext ile veriler alınır.
            // Örnek veri (gerçek uygulamada async API çağrısı yapılmalı)
            ProductList = new ObservableCollection<Products>
            {
                new Products { ProductID = 1, ProductName = "Simit", Price = 5.00M },
                new Products { ProductID = 2, ProductName = "Poğaça", Price = 6.50M },
                new Products { ProductID = 3, ProductName = "Açma", Price = 5.50M },
                // ... veri tabanından gelecek
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
