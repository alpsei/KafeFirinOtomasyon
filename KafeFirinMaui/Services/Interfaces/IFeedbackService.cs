using SharedClass.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeFirinMaui.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<bool> SendFeedbackAsync(FeedBacks feedback);
        Task<List<FeedBacks>> GetFeedbacksAsync();
        Task<List<FeedBacks>> GetFeedbacksByUserId(int userId);
        Task<bool> UpdateFeedbackAsync(FeedBacks feedback);
    }
}
