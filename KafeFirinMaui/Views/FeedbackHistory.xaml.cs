using KafeFirinMaui.ViewModels;
using SharedClass.Classes;

namespace KafeFirinMaui.Views;

public partial class FeedbackHistory : ContentPage
{
	private readonly FeedbackViewModel _viewModel;
    public FeedbackHistory(FeedbackViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        _ = LoadFeedbacksAsync(_viewModel.CurrentCustomerId);
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadFeedbacksAsync(_viewModel.CurrentCustomerId);
    }
    private void FeedbackList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            var selectedFeedback = e.CurrentSelection[0] as FeedBacks;

            if (selectedFeedback != null)
            {
                DetailSubject.Text = $"Konu: {selectedFeedback.Subject}";
                DetailContent.Text = $"İçerik: {selectedFeedback.Content}";
                DetailFbDate.Text = $"Tarih: {selectedFeedback.FBDate}";
                DetailReadReceipt.Text = $"Okundu Bilgisi: {(selectedFeedback.ReadReceipt ? "Okundu" : "Okunmadı")}";
                _viewModel.CurrentCustomerId = selectedFeedback.CustomerID;
                _ = LoadFeedbacksAsync(selectedFeedback.CustomerID);

                DetailPanel.IsVisible = true;
            }
        }
    }
    private async Task LoadFeedbacksAsync(int userId)
    {
        await _viewModel.LoadFeedbacksAsync(userId);

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