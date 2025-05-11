using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedClass.Classes;
using Newtonsoft.Json;
using System.Text.Json;

namespace KafeFirinMaui.Services
{
    public class ProductServices
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ProductServices(HttpClient httpClient, JsonSerializerOptions jsonSerializer)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler);
            _jsonOptions = jsonSerializer;
        }

        public async Task<List<Products>> GetProductsAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:7210/api/products");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Products>>(json);
            }

            return new List<Products>();
        }


    }
}
