using Microsoft.AspNetCore.Mvc;
using CarWorkshopManagementSystem.Services;

namespace CarWorkshopManagementSystem.Controllers;

public class ClientsController : Controller
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    public async Task<IActionResult> Index()
    {
        var clients = await _clientService.GetAllClientsAsync();
        return View(clients);
    }
}