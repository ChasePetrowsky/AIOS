using Microsoft.AspNetCore.Mvc;

public class ServiceController : Controller
{
    public IActionResult Calendar()
    {
        return View();
    }
}
