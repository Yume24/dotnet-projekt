using CarWorkshopManagementSystem.Data;
using CarWorkshopManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshopManagementSystem.Services;

public class VehicleService : IVehicleService
{
    private readonly ApplicationDbContext _context;

    public VehicleService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Vehicle>> GetAllVehiclesAsync()
    {
        return await _context.Vehicles.ToListAsync();
    }
    public async Task<List<Vehicle>> SearchVehiclesAsync(string brand, string model, string plate, int? year, string sortBy, string sortOrder)
    {
        var query = _context.Vehicles.AsQueryable();

        if (!string.IsNullOrWhiteSpace(brand))
            query = query.Where(v => v.Brand.Contains(brand));
        if (!string.IsNullOrWhiteSpace(model))
            query = query.Where(v => v.Model.Contains(model));
        if (!string.IsNullOrWhiteSpace(plate))
            query = query.Where(v => v.LicensePlate.Contains(plate));
        if (year.HasValue)
            query = query.Where(v => v.Year == year.Value);

        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            if (sortOrder == "desc")
                query = query.OrderByDescending(v => EF.Property<object>(v, sortBy));
            else
                query = query.OrderBy(v => EF.Property<object>(v, sortBy));
        }

        return await query.ToListAsync();
    }

    public async Task<Vehicle?> GetVehicleByIdAsync(int id)
    {
        return await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Client?> GetOwnerAsync(int vehicleId)
    {
        var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleId);
        if (vehicle == null) return null;

        return await _context.Clients.FirstOrDefaultAsync(c => c.Id == vehicle.OwnerId);
    }
}