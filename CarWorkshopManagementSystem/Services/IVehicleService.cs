using CarWorkshopManagementSystem.Models;

namespace CarWorkshopManagementSystem.Services;

public interface IVehicleService
{
    Task<List<Vehicle>> GetAllVehiclesAsync();
}