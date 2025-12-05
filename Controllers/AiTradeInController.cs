using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using AIOS.Models;

namespace AIOS.Controllers
{
    [Route("api/ai/tradein")]
    [ApiController]
    public class AITradeInController : ControllerBase
    {
        private readonly string _apiKey;
        private readonly HttpClient _http;

        public AITradeInController(IConfiguration config)
        {
            _apiKey = config["OpenAI:ApiKey"] ?? throw new Exception("OpenAI:ApiKey missing in configuration");

            _http = new HttpClient();
        }

        [HttpPost("estimate")]
        public async Task<IActionResult> Estimate([FromBody] TradeInRequest req)
        {
            // Build prompt for the AI
            var prompt = $@"
You are an automotive trade-in evaluator for a car dealership.

Analyze this vehicle and provide:
1. Estimated wholesale value (in CAD)
2. Trade-in value range (min–max)
3. Recommended trade-in offer
4. A short explanation (2–4 sentences)

Vehicle details:
- Year: {req.Year}
- Make: {req.Make}
- Model: {req.Model}
- Mileage: {req.Mileage} km
- Condition: {req.Condition}
- Notes: {req.Notes}
";

            // Build request body for OpenAI Chat Completions API
            var body = new
            {
                model = "gpt-4.1-mini",   // or "gpt-4.1" if you want the bigger model
                messages = new[]
                {
                    new { role = "system", content = "You are an expert automotive trade-in and wholesale value estimator." },
                    new { role = "user", content = prompt }
                }
            };

            var json = JsonSerializer.Serialize(body);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorText = await response.Content.ReadAsStringAsync();
                // You can log errorText if needed
                return StatusCode((int)response.StatusCode, new { error = "Error calling OpenAI", details = errorText });
            }

            var responseJson = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseJson);
            var root = doc.RootElement;

            // Extract: choices[0].message.content
            string estimateText = root
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "No response from AI";

            return Ok(new { estimate = estimateText });
        }
    }
}
