using CarWorkshopManagementSystem.Models;

namespace CarWorkshopManagementSystem.Services;

public interface IVehicleService
{
    Task<List<Vehicle>> GetAllVehiclesAsync();
    Task<List<Vehicle>> SearchVehiclesAsync(string brand, string model, string plate, int? year, string sortBy, string sortOrder);
    Task<Vehicle?> GetVehicleByIdAsync(int id);
    Task<Client?> GetOwnerAsync(int vehicleId);
    Task<bool> CreateVehicleAsync(Vehicle vehicle, IFormFile? imageFile);
    Task<bool> UpdateVehicleAsync(Vehicle vehicle, IFormFile? imageFile);
    Task<bool> DeleteVehicleAsync(int id);

}