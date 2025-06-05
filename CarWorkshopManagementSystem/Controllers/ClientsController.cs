using CarWorkshopManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using CarWorkshopManagementSystem.Services;
using CarWorkshopManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshopManagementSystem.Controllers;

[Authorize]
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

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Client client)
    {
        if (!ModelState.IsValid)
            return View(client);

        var result = await _clientService.CreateClientAsync(client);
        if (!result)
        {
            ModelState.AddModelError(string.Empty, "Failed to create client.");
            return View(client);
        }

        return RedirectToAction(nameof(Index));
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var client = await _clientService.GetClientByIdAsync(id);
        if (client == null) return NotFound();

        return View(client);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(Client client)
    {
        if (!ModelState.IsValid)
            return View(client);

        var success = await _clientService.UpdateClientAsync(client);
        if (!success)
        {
            ModelState.AddModelError(string.Empty, "Update failed.");
            return View(client);
        }

        return RedirectToAction(nameof(Index));
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var client = await _clientService.GetClientByIdAsync(id);
        if (client == null) return NotFound();

        return View(client); // pokazujemy stronę potwierdzenia
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _clientService.DeleteClientAsync(id);
        if (!result)
        {
            return RedirectToAction(nameof(Index)); // możesz dodać komunikat o błędzie
        }

        return RedirectToAction(nameof(Index));
    }

}