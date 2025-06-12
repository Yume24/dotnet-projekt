using CarWorkshopManagementSystem.Models;
using CarWorkshopManagementSystem.Services;
using CarWorkshopManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
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
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Vehicle vehicle, IFormFile? imageFile)
    {
        if (!ModelState.IsValid) return View(vehicle);

        var success = await _vehicleService.CreateVehicleAsync(vehicle, imageFile);
        if (!success)
            return View(vehicle);

        return RedirectToAction(nameof(Index));
    }


    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
        if (vehicle == null) return NotFound();
        return View(vehicle);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(Vehicle vehicle, IFormFile? imageFile)
    {
        if (!ModelState.IsValid) return View(vehicle);

        var result = await _vehicleService.UpdateVehicleAsync(vehicle, imageFile);
        if (!result) return View(vehicle);

        return RedirectToAction(nameof(Index));
    }
    

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
        if (vehicle == null) return NotFound();

        return View(vehicle);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _vehicleService.DeleteVehicleAsync(id);
        return RedirectToAction(nameof(Index));
    }


}