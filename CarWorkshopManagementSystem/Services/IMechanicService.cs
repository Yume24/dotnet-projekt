using CarWorkshopManagementSystem.Models;

public interface IMechanicService
{
    Task<List<Mechanic>> GetAllAsync();
    Task<Mechanic?> GetByIdAsync(int id);
    Task<bool> CreateAsync(Mechanic mechanic);
    Task<bool> UpdateAsync(Mechanic mechanic);
    Task<bool> DeleteAsync(int id);
    Task<List<Mechanic>> SearchMechanicsAsync(string firstName, string lastName, string email, string phone, string sortBy, string sortOrder);

}