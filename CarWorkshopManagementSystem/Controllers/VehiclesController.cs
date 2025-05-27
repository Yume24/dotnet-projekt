using Microsoft.AspNetCore.Mvc;
using CarWorkshopManagementSystem.Services;

namespace CarWorkshopManagementSystem.Controllers;

public class VehiclesController : Controller
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    public async Task<IActionResult> Index()
    {
        var vehicles = await _vehicleService.GetAllVehiclesAsync();
        return View(vehicles);
    }
}