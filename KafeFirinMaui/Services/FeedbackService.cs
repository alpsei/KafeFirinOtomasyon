using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KafeFirinMaui.Services
{
    public class FeedbackService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        public FeedbackService(HttpClient httpClient, JsonSerializerOptions jsonOptions)
        {
            _httpClient = httpClient;
            _jsonOptions = jsonOptions;
        }
        public async Task<bool> SendFeedbackAsync(string feedback)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { Feedback = feedback }, _jsonOptions), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/feedback", content);
            return response.IsSuccessStatusCode;
        }
    }
}
