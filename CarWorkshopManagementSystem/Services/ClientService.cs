using CarWorkshopManagementSystem.Data;
using CarWorkshopManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshopManagementSystem.Services;

public class ClientService : IClientService
{
    private readonly ApplicationDbContext _context;

    public ClientService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Client>> GetAllClientsAsync()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task<List<Client>> SearchClientsAsync(string name, string surname, string email, string phone, string sortBy, string sortOrder)
    {
        var query = _context.Clients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(c => c.Name.Contains(name));
        if (!string.IsNullOrWhiteSpace(surname))
            query = query.Where(c => c.Surname.Contains(surname));
        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(c => c.Email.Contains(email));
        if (!string.IsNullOrWhiteSpace(phone))
            query = query.Where(c => c.PhoneNumber.Contains(phone));
        
        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            if (sortOrder == "desc")
                query = query.OrderByDescending(c => EF.Property<object>(c, sortBy));
            else
                query = query.OrderBy(c => EF.Property<object>(c, sortBy));
        }

        return await query.ToListAsync();
    }
    public async Task<Client?> GetClientByIdAsync(int id)
    {
        return await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Vehicle>> GetVehiclesForClientAsync(int clientId)
    {
        return await _context.Vehicles
            .Where(v => v.OwnerId == clientId)
            .ToListAsync();
    }
    public async Task<bool> CreateClientAsync(Client client)
    {
        try
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    public async Task<bool> UpdateClientAsync(Client client)
    {
        try
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    public async Task<bool> DeleteClientAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null) return false;

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return true;
    }

}