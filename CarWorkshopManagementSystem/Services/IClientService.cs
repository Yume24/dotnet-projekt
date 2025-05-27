using CarWorkshopManagementSystem.Models;

namespace CarWorkshopManagementSystem.Services;

public interface IClientService
{
    Task<List<Client>> GetAllClientsAsync();
}