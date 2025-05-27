using CarWorkshopManagementSystem.Models;

namespace CarWorkshopManagementSystem.ViewModels;

public class ClientVehiclesViewModel
{
    public Client Client { get; set; }
    public List<Vehicle> Vehicles { get; set; }
}