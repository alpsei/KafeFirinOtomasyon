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

        public OrderService(HttpClient httpClient, JsonSerializerOptions jsonSerializer)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler);
            _jsonOptions = jsonSerializer;
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
            var response = await _httpClient.GetAsync("https://localhost:7210/api/orders");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Orders>>(json);
            }

            return new List<Orders>();
        }

    }
}
