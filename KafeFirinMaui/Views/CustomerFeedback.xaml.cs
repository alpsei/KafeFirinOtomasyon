using KafeFirinMaui.Services;
using KafeFirinMaui.ViewModels;

namespace KafeFirinMaui.Views;

public partial class CustomerFeedback : ContentPage
{
    private readonly FeedbackViewModel _viewModel;

    public CustomerFeedback(FeedbackService feedbackService)
    {
        InitializeComponent();
        _viewModel = new FeedbackViewModel(feedbackService);
        BindingContext = _viewModel;
    }

    private async void OnSendButtonTapped(object sender, EventArgs e)
    {
        var result = await _viewModel.SendFeedbackAsync();
        if (result)
        {
            await DisplayAlert("Baþarýlý", "Geri bildiriminiz alýndý.", "Tamam");
        }
        else
        {
            await DisplayAlert("Hata", "Lütfen boþ býrakmayýnýz.", "Tamam");
        }
    }

}

