using System.Threading.Tasks;
using AIOS.Models;
using OpenAI.Chat;

namespace AIOS.Repositories
{
    public class AiPricingRepository
    {
        private readonly ChatClient _chatClient;

        public AiPricingRepository(IConfiguration config)
        {
            var apiKey = config["OpenAI:ApiKey"];
            _chatClient = new ChatClient(
                model: "gpt-4o-mini",
                apiKey: apiKey
            );
        }

        public async Task<string> GetEstimatedPriceAsync(VehiclePricingRequest r)
        {
            var prompt =
                $"Estimate the fair market value in USD for a {r.Year} {r.Make} {r.Model} with {r.Mileage} km. " +
                "Respond with ONLY a number (no dollar sign, no commas). Example: 18200";

            ChatCompletion completion = await _chatClient.CompleteChatAsync(prompt);

            // Grab the text from the first content block
            var text = completion.Content[0].Text.Trim();

            return text;
        }
    }
}
