using SharedClasses;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace KafeFirinMaui.Views;

public partial class CustomerOrders : ContentPage
{
    public ObservableCollection<Products> Products { get; set; }
    public ObservableCollection<Products> Cart { get; set; }

    public ICommand AddToCartCommand { get; set; }
    public CustomerOrders()
    {
        InitializeComponent();
        Products = new ObservableCollection<Products>(GetProductsFromDatabase());
        Cart = new ObservableCollection<Products>();

        AddToCartCommand = new Command<Products>((product) =>
        {
            Cart.Add(product);
        });
    }
    private List<Products> GetProductsFromDatabase()
    {
        using (var db = new AppDb())
        {
            return db.Products.ToList();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
}