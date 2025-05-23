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
            await DisplayAlert("Ba�ar�l�", "Geri bildiriminiz al�nd�.", "Tamam");
        }
        else
        {
            await DisplayAlert("Hata", "L�tfen bo� b�rakmay�n�z.", "Tamam");
        }
    }

}

