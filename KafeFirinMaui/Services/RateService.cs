using KafeFirinMaui.Helpers;
using SharedClass.Classes;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace KafeFirinMaui.Services
{
    public class RateService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly ILogger<RateService> _logger;

        public RateService(IHttpClientFactory httpClientFactory, JsonSerializerOptions jsonOptions, ILogger<RateService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _jsonOptions = jsonOptions;
            _logger = logger;
        }

        public async Task<List<Rates>> GetRatesByEmployeeIDAsync(int employeeId)
        {
            var requestUri = $"/rate/employee/{employeeId}";
            _logger.LogInformation("[RateService] GetRatesByEmployeeIDAsync çağrıldı. EmployeeID: {EmployeeId}, İstek URI: {RequestUri}", employeeId, _httpClient.BaseAddress + requestUri);

            try
            {
                var response = await _httpClient.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("[RateService] GetRatesByEmployeeIDAsync başarılı. Yanıt: {Json}", json);
                    var ratesList = JsonSerializer.Deserialize<List<Rates>>(json, _jsonOptions);
                    return ratesList ?? new List<Rates>();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("[RateService] GetRatesByEmployeeIDAsync başarısız. Statü Kodu: {StatusCode}, Yanıt: {ErrorContent}, İstek URI: {RequestUri}", response.StatusCode, errorContent, _httpClient.BaseAddress + requestUri);
                    return new List<Rates>();
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "[RateService] GetRatesByEmployeeIDAsync HttpRequestException. Mesaj: {Message}, İstek URI: {RequestUri}", httpEx.Message, _httpClient.BaseAddress + requestUri);
                return new List<Rates>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RateService] GetRatesByEmployeeIDAsync genel hata. Mesaj: {Message}, İstek URI: {RequestUri}", ex.Message, _httpClient.BaseAddress + requestUri);
                return new List<Rates>();
            }
        }

        public async Task<bool> SaveRateAsync(Rates rate)
        {


            var response = await _httpClient.PostAsJsonAsync("/rate", rate, _jsonOptions);

            string jsonPayload = "";
            try
            {
                jsonPayload = JsonSerializer.Serialize(rate, _jsonOptions);
                var jsonContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                _logger.LogInformation("[RateService] SaveRateAsync çağrılıyor. Payload: {JsonPayload}", jsonPayload);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("[RateService] SaveRateAsync başarılı. Statü Kodu: {StatusCode}, Yanıt: {ResponseContent}", response.StatusCode, responseContent);
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("[RateService] SaveRateAsync API'den başarısız yanıt aldı. Statü Kodu: {StatusCode}, Hata İçeriği: {ErrorContent}, Payload: {JsonPayload}",
                                       response.StatusCode, errorContent, jsonPayload);
                    return false;
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "[RateService] SaveRateAsync HttpRequestException. Mesaj: {Message}, Payload: {JsonPayload}", httpEx.Message, jsonPayload);
                return false;
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "[RateService] SaveRateAsync JsonException (serileştirme hatası). Mesaj: {Message}, Data: Rate={Rate}",
                                 jsonEx.Message, rate);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RateService] SaveRateAsync genel hata. Mesaj: {Message}, Payload: {JsonPayload}", ex.Message, jsonPayload);
                return false;
            }
        }
    }
}
