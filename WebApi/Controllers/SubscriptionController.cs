using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class SubscriptionController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public SubscriptionController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpPost("check-subscription")]
        public async Task<IActionResult> CheckSubscription([FromBody] NameRequest request)
        {
            try
            {
                
                var json = JsonSerializer.Serialize(new
                {
                    name = request.Name,
                    deviceId = request.DeviceId  
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("http://localhost:8000/server/", content);
                var result = await response.Content.ReadAsStringAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error communicating with WPF server: {ex.Message}");
            }
        }

        public class NameRequest
        {
            public string Name { get; set; }
            public string DeviceId { get; set; }  
        }
    }
}