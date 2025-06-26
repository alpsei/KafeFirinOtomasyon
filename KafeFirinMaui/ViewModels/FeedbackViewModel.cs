using KafeFirinMaui.Helpers;
using KafeFirinMaui.Services.Interfaces;
using SharedClass.Classes;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace KafeFirinMaui.ViewModels
{
    public class FeedbackViewModel : INotifyPropertyChanged
    {
        private readonly IFeedbackService _feedbackService;
        private List<FeedBacks> _feedbacks = new List<FeedBacks>();
        public List<FeedBacks> FeedBacks
        {
            get => _feedbacks;
            set
            {
                _feedbacks = value;
                OnPropertyChanged();
            }
        }
        private FeedBacks _feedback;
        public FeedBacks Feedback
        {
            get => _feedback;
            set { _feedback = value; OnPropertyChanged(); }
        }
        private string _selectedTopic;
        public string SelectedTopic
        {
            get => _selectedTopic;
            set
            {
                _selectedTopic = value;
                OnPropertyChanged();
            }
        }

        private string _feedbackMessage;
        public string FeedbackMessage
        {
            get => _feedbackMessage;
            set
            {
                _feedbackMessage = value;
                OnPropertyChanged();
            }
        }

        private int _currentCustomerId;
        public int CurrentCustomerId
        {
            get => _currentCustomerId;
            set
            {
                if (_currentCustomerId != value)
                {
                    _currentCustomerId = value;
                    OnPropertyChanged();
                }
            }
        }

        public FeedbackViewModel(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
            _feedback = new FeedBacks();
            CurrentCustomerId = Session.LoggedInUser?.UserID ?? 0;
        }

        public async Task<bool> SendFeedbackAsync()
        {
            if (string.IsNullOrWhiteSpace(FeedbackMessage) || string.IsNullOrWhiteSpace(SelectedTopic))
                return false;

            _feedback = new FeedBacks
            {
                CustomerID = CurrentCustomerId,
                Subject = SelectedTopic,
                Content = FeedbackMessage,
                FBDate = DateTime.Now,
                ReadReceipt = false
            };

            var result = await _feedbackService.SendFeedbackAsync(_feedback);
            if (result)
            {
                FeedbackMessage = string.Empty;
                SelectedTopic = null;
                return true;
            }

            return false;
        }
        public async Task<bool> MarkFeedbackAsReadAsync(FeedBacks feedback)
        {
            feedback.ReadReceipt = true;
            return await _feedbackService.UpdateFeedbackAsync(feedback);
        }

        public async Task LoadFeedbacksAsync(int userId)
        {
            var feedbackList = await _feedbackService.GetFeedbacksByUserId(CurrentCustomerId);
            FeedBacks = feedbackList ?? new List<FeedBacks>();

            if (feedbackList != null && feedbackList.Any())
            {
                var specificFeedback = feedbackList.FirstOrDefault(f => f.CustomerID == CurrentCustomerId);
                if (specificFeedback != null)
                {
                    SelectedTopic = specificFeedback.Subject;

                }
                CurrentCustomerId = userId;
            }
            OnPropertyChanged(nameof(SelectedTopic));
        }
        public async Task LoadFeedbacksAsyncAsManager()
        {
            var allFeedbacks = await _feedbackService.GetFeedbacksAsync();
            FeedBacks = allFeedbacks;
            OnPropertyChanged(nameof(FeedBacks));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
