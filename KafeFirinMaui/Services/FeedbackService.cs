using Microsoft.Extensions.Logging;
using SharedClass.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KafeFirinMaui.Services
{
    public class FeedbackService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly ILogger<UserService> _logger;
        public FeedbackService(IHttpClientFactory httpClientFactory, JsonSerializerOptions jsonOptions, ILogger<UserService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _jsonOptions = jsonOptions;
            _logger = logger;
        }
        public async Task<bool> SendFeedbackAsync(FeedBacks feedback)
        {
            var content = new StringContent(JsonSerializer.Serialize(feedback, _jsonOptions), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/feedback", content);
            if(response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return true;
            }

            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return false;
            }
        }
        public async Task<List<FeedBacks>> GetFeedbacksAsync()
        {
            const string requestUri = "/feedback";
            _logger.LogInformation("[FeedbackService] GetFeedbacksAsync çağrıldı. İstek URI: {RequestUri}", _httpClient.BaseAddress + requestUri);

            try
            {
                var response = await _httpClient.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("[FeedbackService] GetFeedbacksAsync başarılı. Yanıt: {Json}", json);
                    var feedbackList = JsonSerializer.Deserialize<List<FeedBacks>>(json, _jsonOptions);
                    return feedbackList ?? new List<FeedBacks>();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("[FeedbackService] GetFeedbacksAsync başarısız. Statü Kodu: {StatusCode}, Yanıt: {ErrorContent}, İstek URI: {RequestUri}", response.StatusCode, errorContent, _httpClient.BaseAddress + requestUri);
                    return new List<FeedBacks>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[FeedbackService] GetFeedbacksAsync genel hata. Mesaj: {Message}, İstek URI: {RequestUri}", ex.Message, _httpClient.BaseAddress + requestUri);
                return new List<FeedBacks>();
            }
        }

        public async Task<List<FeedBacks>> GetFeedbacksByUserId(int userId)
        {
            var requestUri = $"/feedback/user/{userId}";
            _logger.LogInformation("[FeedbackService] GetFeedbacksByUserId çağrıldı. UserId: {UserId}, İstek URI: {RequestUri}", userId, _httpClient.BaseAddress + requestUri);

            try
            {
                var response = await _httpClient.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("[FeedbackService] GetFeedbacksByUserId başarılı. Yanıt: {Json}", json);
                    var feedbackList = JsonSerializer.Deserialize<List<FeedBacks>>(json, _jsonOptions);
                    return feedbackList ?? new List<FeedBacks>();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("[FeedbackService] GetFeedbacksByUserId başarısız. Statü Kodu: {StatusCode}, Yanıt: {ErrorContent}, İstek URI: {RequestUri}", response.StatusCode, errorContent, _httpClient.BaseAddress + requestUri);
                    return new List<FeedBacks>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[FeedbackService] GetFeedbacksByUserId genel hata. Mesaj: {Message}, İstek URI: {RequestUri}", ex.Message, _httpClient.BaseAddress + requestUri);
                return new List<FeedBacks>();
            }
        }
        public async Task<bool> UpdateFeedbackAsync(FeedBacks feedback)
        {
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(feedback), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/feedback/{feedback.FeedBackID}", content);
            return response.IsSuccessStatusCode;
        }

    }
}
