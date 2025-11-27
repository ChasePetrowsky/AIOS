using Microsoft.AspNetCore.Mvc;

public class CustomersController : Controller
{
    public IActionResult Index() => View();     // Customer list
    public IActionResult Profile() => View();   // Customer profile
}
