using Microsoft.AspNetCore.Mvc;
using CarWorkshopManagementSystem.Models;

namespace CarWorkshopManagementSystem.Controllers;

public class ClientsController : Controller
{
    public IActionResult Overview()
    {
        var client = new Client() { Id = 0, Email = "test", Name = "jan", Surname = "doe" , PhoneNumber = "342"};
        return View(client);
    }

    public IActionResult Details(int id)
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    public IActionResult Edit(int id)
    {
        return View();
    }
}