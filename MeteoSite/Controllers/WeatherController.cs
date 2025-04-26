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
        private readonly IConfiguration _config;
        private readonly HttpClient _http;

        public WeatherController(IConfiguration config)
        {
            _config = config;
            _http = new HttpClient();
        }

        [HttpGet("{lat}/{lon}")]
        public async Task<IActionResult> GetWeather(double lat, double lon)
        {
            var apiKey = _config["OpenWeatherMap:ApiKey"];
            var url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKey}&units=metric&lang=fr";

            var resp = await _http.GetAsync(url);
            if (!resp.IsSuccessStatusCode)
                return StatusCode((int)resp.StatusCode, "Erreur appel API météo");

            var json = await resp.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }
    }
}
