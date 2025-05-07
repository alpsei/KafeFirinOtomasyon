using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using SharedClasses;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace KafeFirinMaui.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserService> _logger;

        public UserService(IHttpClientFactory httpClientFactory, ILogger<UserService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _logger = logger;
        }

        private JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public async Task<List<Users>> GetUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/users");
                response.EnsureSuccessStatusCode();
                var users = await response.Content.ReadFromJsonAsync<List<Users>>(_jsonOptions);
                return users ?? new List<Users>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcılar alınırken hata oluştu");
                await App.Current.MainPage.DisplayAlert("Hata", "Kullanıcılar alınamadı.", "Tamam");
                return new List<Users>();
            }
        }

        public async Task<bool> CreateUsersAsync(Users user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/users", user);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı oluşturulurken hata oluştu");
                await App.Current.MainPage.DisplayAlert("Hata", "Kullanıcı oluşturulamadı.", "Tamam");
                return false;
            }
        }

        public async Task<bool> UpdateUsersAsync(Users user)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/api/users/{user.UserID}", user);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı güncellenirken hata oluştu");
                await App.Current.MainPage.DisplayAlert("Hata", "Kullanıcı güncellenemedi.", "Tamam");
                return false;
            }
        }

        public async Task<bool> DeleteUsersAsync(int userId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/users/{userId}");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı silinirken hata oluştu");
                await App.Current.MainPage.DisplayAlert("Hata", "Kullanıcı silinemedi.", "Tamam");
                return false;
            }
        }

        public async Task<Users> GetUserByIdAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/users/{userId}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<Users>(json, _jsonOptions);
                return user;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Kullanıcı alınırken API hatası");
                await App.Current.MainPage.DisplayAlert("Hata", "Kullanıcı bilgileri alınamadı.", "Tamam");
                return null;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserileştirme hatası");
                await App.Current.MainPage.DisplayAlert("Hata", "Veri işlenirken bir hata oluştu.", "Tamam");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı alınırken beklenmedik hata");
                await App.Current.MainPage.DisplayAlert("Hata", "Bilinmeyen bir hata oluştu.", "Tamam");
                return null;
            }
        }
        public async Task<Users> UserLogin(string username, string password)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/users/login?username={username}&password={password}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<Users>(json, _jsonOptions);
                return user;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Kullanıcı girişi sırasında API hatası");
                await App.Current.MainPage.DisplayAlert("Hata", "Kullanıcı girişi başarısız.", "Tamam");
                return null;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserileştirme hatası");
                await App.Current.MainPage.DisplayAlert("Hata", "Veri işlenirken bir hata oluştu.", "Tamam");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı girişi sırasında beklenmedik hata");
                await App.Current.MainPage.DisplayAlert("Hata", "Bilinmeyen bir hata oluştu.", "Tamam");
                return null;
            }
        }
    }
}

