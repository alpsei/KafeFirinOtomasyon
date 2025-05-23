using KafeFirinMaui.ViewModels;
using SharedClass.Classes;

namespace KafeFirinMaui.Views;

public partial class ShowFeedbacks : ContentPage
{
    private readonly FeedbackViewModel _viewModel;
    public ShowFeedbacks(FeedbackViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        _ = LoadFeedbacksAsync();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadFeedbacksAsyncAsManager();
    }
    private async void FeedbackList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            var selectedFeedback = e.CurrentSelection[0] as FeedBacks;

            if (selectedFeedback != null)
            {
                if (!selectedFeedback.ReadReceipt)
                {
                    await _viewModel.MarkFeedbackAsReadAsync(selectedFeedback);
                }

                DetailSubject.Text = $"Konu: {selectedFeedback.Subject}";
                DetailContent.Text = $"Ýçerik: {selectedFeedback.Content}";
                DetailFbDate.Text = $"Tarih: {selectedFeedback.FBDate}";
                DetailReadReceipt.Text = $"Okundu Bilgisi: {(selectedFeedback.ReadReceipt ? "Okundu" : "Okunmadý")}";
                DetailPanel.IsVisible = true;
            }
        }
    }
    private async Task LoadFeedbacksAsync()
    {
        await _viewModel.LoadFeedbacksAsyncAsManager();

        if (_viewModel.FeedBacks != null && _viewModel.FeedBacks.Count > 0)
        {
            string selectedTopic = _viewModel.SelectedTopic;
            DetailSubject.Text = $"Konu: {selectedTopic}";
        }
        else
        {
            DetailSubject.Text = "Henüz geri bildiriminiz yok.";
        }
    }
}