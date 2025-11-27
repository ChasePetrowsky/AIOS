using Microsoft.AspNetCore.Mvc;

public class InventoryController : Controller
{
    public IActionResult Index() => View();     // Vehicle list UI
    public IActionResult Add() => View();       // Add vehicle form UI
    public IActionResult Details() => View();   // Vehicle details UI
}
