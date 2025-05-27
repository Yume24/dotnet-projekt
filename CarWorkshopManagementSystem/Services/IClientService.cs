using CarWorkshopManagementSystem.Models;

namespace CarWorkshopManagementSystem.Services;

public interface IClientService
{
    Task<List<Client>> GetAllClientsAsync();
    Task<List<Client>> SearchClientsAsync(string name, string surname, string email, string phone, string sortBy, string sortOrder);
    Task<Client?> GetClientByIdAsync(int id);
    Task<List<Vehicle>> GetVehiclesForClientAsync(int clientId);
}