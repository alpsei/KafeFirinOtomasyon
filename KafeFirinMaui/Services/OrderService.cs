using Newtonsoft.Json;
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
    public class OrderService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public OrderService(IHttpClientFactory httpClientFactory, JsonSerializerOptions jsonSerializer)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _jsonOptions = jsonSerializer;
        }

        public async Task<bool> PlaceOrder(OrderRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/orders", request);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Hata Kodu: {response.StatusCode}");
                    Console.WriteLine($"Hata İçeriği: {responseBody}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"İstisna: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Orders>> GetOrdersAsync()
        {
            var response = await _httpClient.GetAsync("/api/orders");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Orders>>(json);
            }

            return new List<Orders>();
        }
        public class OrdersResponse
        {
            public List<Orders> Orders { get; set; }
        }

        public async Task<List<Orders>> GetOrdersByStaffIdAsync(int staffId)
        {
            var response = await _httpClient.GetAsync($"/api/orders/staff/{staffId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Orders>>(json);
            }
            return new List<Orders>();
        }



        public async Task<bool> UpdateOrderStatusAsync(Orders orders)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/api/orders/{orders.OrderID}", orders);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Hata", "Kullanıcı güncellenemedi.", "Tamam");
                return false;
            }
        }


        public async Task<int> GetUserOrderCountAsync(int userId)
        {
            var orders = await GetOrdersAsync();
            return orders.Count(o => o.CustomerID == userId);
        }


    }
}
