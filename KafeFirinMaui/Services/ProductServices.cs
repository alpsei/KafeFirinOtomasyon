using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedClass.Classes;
using Newtonsoft.Json;
using System.Text.Json;
using System.Net.Http.Json;

namespace KafeFirinMaui.Services
{
    public class ProductServices
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ProductServices(IHttpClientFactory httpClientFactory, JsonSerializerOptions jsonSerializer)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _jsonOptions = jsonSerializer;
        }

        public async Task<List<Products>> GetProductsAsync()
        {
            var response = await _httpClient.GetAsync("/api/products");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Products>>(json);
            }

            return new List<Products>();
        }

        public async Task<bool> AddProductAsync(Products product)
        {
            var json = JsonConvert.SerializeObject(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/products", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProductAsync(Products products)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/api/products/{products.ProductID}", products);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Hata", "Ürün güncellenemedi.", "Tamam");
                return false;
            }
        }


    }
}
