using System.Net.Http.Json;

namespace SmartShelf.web.Services
{
    public class EmailService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public EmailService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task SendLowStockEmailAsync(string productName, int currentStock, int threshold)
        {
            var payload = new
            {
                service_id = _config["EmailJs:ServiceId"],
                template_id = _config["EmailJs:TemplateId"],
                user_id = _config["EmailJs:PublicKey"],
                accessToken = _config["EmailJs:PrivateKey"],

                template_params = new
                {
                    to_email = _config["EmailJs:ToEmail"],
                    product_name = productName,
                    current_stock = currentStock,
                    threshold = threshold,
                    message = $"{productName} is low in stock. Current stock: {currentStock}, threshold: {threshold}."
                }
            };

            var response = await _httpClient.PostAsJsonAsync(
                "https://api.emailjs.com/api/v1.0/email/send",
                payload
            );

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"EmailJS failed: {response.StatusCode} - {error}");
            }
        }
    }
}