// Services/FavoriteService.cs

using KafeFirinMaui.Services.Interfaces;
using SharedClass.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KafeFirinMaui.Services.Classes
{
    public class FavoriteService : IFavoriteService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public FavoriteService(IHttpClientFactory httpClientFactory, JsonSerializerOptions jsonOptions)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _jsonOptions = jsonOptions;
        }

        public async Task<List<Favorites>> GetFavoritesAsync()
        {
            const string requestUri = "/favorite";
            try
            {
                var response = await _httpClient.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var favoriteList = JsonSerializer.Deserialize<List<Favorites>>(json, _jsonOptions);
                    return favoriteList ?? new List<Favorites>();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new List<Favorites>();
                }
            }
            catch (Exception ex)
            {
                return new List<Favorites>();
            }
        }

        public async Task<bool> SendFavoritesAsync(Favorites favorite)
        {
            var content = new StringContent(JsonSerializer.Serialize(favorite, _jsonOptions), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/favorite", content);
            if (response.IsSuccessStatusCode)
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

        public async Task<bool> DeleteFavoriteByUserIdAsync(int userId, int productId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/favorite/by-userid/{userId}/{productId}");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}