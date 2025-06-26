using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using SharedClass.Classes;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using KafeFirinMaui.Services.Interfaces;

namespace KafeFirinMaui.Services.Classes
{
    public class UserService : IUserService
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
                await Application.Current.MainPage.DisplayAlert("Hata", "Kullanıcılar alınamadı.", "Tamam");
                return new List<Users>();
            }
        }

        public async Task<bool> CreateUsersAsync(Users user, int? createdBy = null)
        {
            try
            {
                string url = "/api/users";
                if (createdBy != null)
                {
                    url += $"?createdBy={createdBy}";
                }

                var response = await _httpClient.PostAsJsonAsync(url, user);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı oluşturulurken hata oluştu");
                await Application.Current.MainPage.DisplayAlert("Hata", "Kullanıcı oluşturulamadı.", "Tamam");
                return false;
            }
        }


        public async Task<bool> UpdateUsersAsync(Users user, int? updatedBy)
        {
            try
            {
                string url = $"/api/users/{user.UserID}";
                if (updatedBy!= null)
                {
                    url += $"?updatedBy={updatedBy}";
                }

                var response = await _httpClient.PutAsJsonAsync(url, user);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı güncellenirken hata oluştu");
                await Application.Current.MainPage.DisplayAlert("Hata", "Kullanıcı güncellenemedi.", "Tamam");
                return false;
            }
        }

        public async Task<bool> DeleteUsersAsync(int userId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/users/by-id/{userId}");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı silinirken hata oluştu");
                await Application.Current.MainPage.DisplayAlert("Hata", "Kullanıcı silinemedi.", "Tamam");
                return false;
            }
        }
        public async Task<bool> DeleteUsersAsync(string username)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/users/by-username/{username}");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı silinirken hata oluştu");
                await Application.Current.MainPage.DisplayAlert("Hata", "Kullanıcı silinemedi.", "Tamam");
                return false;
            }
        }

        public async Task<Users> GetUserByIdAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/users/by-id/{userId}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<Users>(json, _jsonOptions);
                return user;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Kullanıcı alınırken API hatası");
                await Application.Current.MainPage.DisplayAlert("Hata", "Kullanıcı bilgileri alınamadı.", "Tamam");
                return null;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserileştirme hatası");
                await Application.Current.MainPage.DisplayAlert("Hata", "Veri işlenirken bir hata oluştu.", "Tamam");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı alınırken beklenmedik hata");
                await Application.Current.MainPage.DisplayAlert("Hata", "Bilinmeyen bir hata oluştu.", "Tamam");
                return null;
            }
        }
        public async Task<Users> GetUserByUsernameAsync(string username)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/users/by-username/{username}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<Users>(json, _jsonOptions);
                return user;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Kullanıcı alınırken API hatası");
                await Application.Current.MainPage.DisplayAlert("Hata", "Kullanıcı bilgileri alınamadı.", "Tamam");
                return null;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserileştirme hatası");
                await Application.Current.MainPage.DisplayAlert("Hata", "Veri işlenirken bir hata oluştu.", "Tamam");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı alınırken beklenmedik hata");
                await Application.Current.MainPage.DisplayAlert("Hata", "Bilinmeyen bir hata oluştu.", "Tamam");
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
                await Application.Current.MainPage.DisplayAlert("Hata", "Kullanıcı girişi başarısız.", "Tamam");
                return null;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserileştirme hatası");
                await Application.Current.MainPage.DisplayAlert("Hata", "Veri işlenirken bir hata oluştu.", "Tamam");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı girişi sırasında beklenmedik hata");
                await Application.Current.MainPage.DisplayAlert("Hata", "Bilinmeyen bir hata oluştu.", "Tamam");
                return null;
            }
        }
        public async Task<bool> UpdatePasswordAsync(string username, string secQuestion, string secAnswer, string newPassword)
        {
            try
            {
                var user = await GetUserByUsernameAsync(username);
                if (user == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Hata", "Kullanıcı bulunamadı.", "Tamam");
                    return false;
                }

                if (!string.Equals(user.SecQuestion, secQuestion, StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(user.SecAnswer, secAnswer, StringComparison.OrdinalIgnoreCase))
                {
                    await Application.Current.MainPage.DisplayAlert("Hata", "Güvenlik sorusu veya cevabı hatalı.", "Tamam");
                    return false;
                }

                user.Password = newPassword;

                var response = await _httpClient.PutAsJsonAsync($"/api/users/{user.UserID}", user);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Şifre güncellenirken hata oluştu");
                await Application.Current.MainPage.DisplayAlert("Hata", "Şifre güncellenemedi.", "Tamam");
                return false;
            }
        }
        public async Task<Users?> GetUserByUsernameAndSecurity(string username, string secQuestion, string secAnswer)
        {
            var user = await GetUserByUsernameAsync(username);
            if (user == null)
                return null;
            if (!string.Equals(user.SecQuestion, secQuestion, StringComparison.OrdinalIgnoreCase) ||
                !string.Equals(user.SecAnswer, secAnswer, StringComparison.OrdinalIgnoreCase))
                return null;

            return user;
        }


    }
}

