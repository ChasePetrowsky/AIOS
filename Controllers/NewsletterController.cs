using Microsoft.AspNetCore.Mvc;
using AIOS.Repositories;
using AIOS.Models;

[ApiController]
[Route("api/[controller]")]
public class NewsletterController : ControllerBase
{
    private readonly NewsletterRepository _repo;

    public NewsletterController(NewsletterRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("latest")]
    public async Task<IActionResult> Latest()
    {
        var latest = await _repo.GetLatestAsync();
        return Ok(latest);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var all = await _repo.GetAllAsync();
        return Ok(all);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Newsletter newsletter)
    {
        newsletter.PublishedOn = DateTime.UtcNow;
        await _repo.CreateAsync(newsletter);
        return Ok(newsletter);
    }
}
