using System.Net.Http.Json;
using System.Text.Json;

namespace WebApplication1.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "K8Bxfv63MRrd6KszrLTL8iuEzDzhwT";

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetCurrencyConversion(string from, string to, decimal amount)
        {
            string url = $"https://www.amdoren.com/api/currency.php?api_key={_apiKey}&from={from}&to={to}&amount={amount}";
            var response = await _httpClient.GetFromJsonAsync<ApiResponse>(url);

            if (response != null && response.error == 0)
            {
                return response.amount;  // Ensure only the amount is returned
            }

            throw new Exception("Failed to retrieve currency data");
        }

    }


    public class ApiResponse
    {
        public int error { get; set; }
        public decimal amount { get; set; }
    }

}
