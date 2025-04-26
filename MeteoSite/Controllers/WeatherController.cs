using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace MeteoSite.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public WeatherController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        [HttpGet("{lat}/{lon}")]
        public async Task<IActionResult> GetWeather(double lat, double lon)
        {
            var apiKey = _configuration["OpenWeatherMap:ApiKey"];
            var url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKey}&units=metric&lang=fr";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Erreur appel API météo");
            }

            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }
    }
}
