using Microsoft.AspNetCore.Mvc;
using AIOS.Repositories;
using AIOS.Models;

namespace AIOS.Controllers
{
    [ApiController]
    [Route("api/ai-pricing")]
    public class AiPricingController : ControllerBase
    {
        private readonly AiPricingRepository _repo;

        public AiPricingController(AiPricingRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("estimate")]
        public async Task<IActionResult> EstimatePrice([FromBody] VehiclePricingRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request data.");

            var result = await _repo.GetEstimatedPriceAsync(request);

            return Ok(new { price = result });
        }
    }
}
