using ABI.System.Collections.Generic;
using KafeFirinMaui.Helpers;
using KafeFirinMaui.Services;
using KafeFirinMaui.ViewModels;
using SharedClass.Classes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KafeFirinMaui.Views;

public partial class CustomerOrders : ContentPage
{
    private readonly CustomerOrdersViewModel _viewModel;
    private readonly FavoriteService _favoriteService;
    public CustomerOrders(CustomerOrdersViewModel viewModel, FavoriteService favoriteService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _favoriteService = favoriteService;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadProductsAsync();
    }
    public async void OnHeartClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.BindingContext is ProductViewModel product)
        {
            var vm = BindingContext as CustomerOrdersViewModel;
            if (vm == null) return;

            if (button.ClassId == "empty")
            {
                var favorite = new Favorites
                {
                    CustomerID = Session.LoggedInUser.UserID,
                    ProductID = product.ProductID,
                };
                var result = await _favoriteService.SendFavoritesAsync(favorite);
                if (result)
                {
                    product.IsFavorite = true;
                    if (!vm.FavoriteProductsId.Contains(product.ProductID))
                    {
                        vm.FavoriteProductsId.Add(product.ProductID);
                        vm.ResortProductList();
                    }
                }
                else
                {
                    await DisplayAlert("Hata", "Favoriye eklenemedi.", "Tamam");
                    Console.WriteLine($"[Favori Sil] UserID: {Session.LoggedInUser?.UserID}, ProductID: {product?.ProductID}");
                }
            }
            else
            {
                var result = await _favoriteService.DeleteFavoriteByUserIdAsync(Session.LoggedInUser.UserID, product.ProductID);
                if (result)
                {
                    product.IsFavorite = false;
                    if (vm.FavoriteProductsId.Contains(product.ProductID))
                    {
                        vm.FavoriteProductsId.Remove(product.ProductID);
                        vm.ResortProductList();
                    }
                }
                else
                {
                    await DisplayAlert("Hata", "Favoriden silinemedi.", "Tamam");
                }
            }
        }
    }
}