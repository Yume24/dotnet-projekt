using Microsoft.AspNetCore.Mvc;
using CarWorkshopManagementSystem.Services;
using CarWorkshopManagementSystem.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshopManagementSystem.Controllers;

public class ClientsController : Controller
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    public async Task<IActionResult> Index(string name, string surname, string email, string phone, string sortBy = "Id", string sortOrder = "asc")
    {
        var clients = await _clientService.SearchClientsAsync(name, surname, email, phone, sortBy, sortOrder);
        return View(clients);
    }
    public async Task<IActionResult> Vehicles(int id)
    {
        var client = await _clientService.GetClientByIdAsync(id);
        if (client == null)
            return NotFound();

        var vehicles = await _clientService.GetVehiclesForClientAsync(id);

        var viewModel = new ClientVehiclesViewModel
        {
            Client = client,
            Vehicles = vehicles
        };

        return View(viewModel);
    }


}