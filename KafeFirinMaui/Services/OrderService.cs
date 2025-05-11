using Newtonsoft.Json;
using SharedClass.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace KafeFirinMaui.Services
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> PlaceOrder(OrderRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7210/api/orders", request);
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
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7210/api/orders");
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<Orders>>(jsonResponse);
                return orders;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Siparişleri yüklerken hata: {ex.Message}");
                return new List<Orders>();
            }
        }

    }
}
