using CarWorkshopManagementSystem.Models;
using CarWorkshopManagementSystem.Services;
using CarWorkshopManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

public class VehiclesController : Controller
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    public async Task<IActionResult> Index(string brand, string model, string plate, int? year, string sortBy = "Id", string sortOrder = "asc")
    {
        var vehicles = await _vehicleService.SearchVehiclesAsync(brand, model, plate, year, sortBy, sortOrder);
        return View(vehicles);
    }

    public async Task<IActionResult> Owner(int id)
    {
        var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
        if (vehicle == null) return NotFound();

        var owner = await _vehicleService.GetOwnerAsync(id);
        if (owner == null) return NotFound();

        var vm = new ClientVehiclesViewModel
        {
            Client = owner,
            Vehicles = new List<Vehicle> { vehicle }
        };

        return View(vm);
    }


}