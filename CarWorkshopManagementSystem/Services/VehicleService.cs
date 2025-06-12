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
    private async Task<string?> SaveImageAsync(IFormFile? imageFile)
    {
        if (imageFile == null || imageFile.Length == 0) return null;

        var uploadsPath = Path.Combine("wwwroot", "uploads");
        if (!Directory.Exists(uploadsPath))
            Directory.CreateDirectory(uploadsPath);

        var uniqueFileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
        var filePath = Path.Combine(uploadsPath, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        return "/uploads/" + uniqueFileName;
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
    public async Task<bool> CreateVehicleAsync(Vehicle vehicle, IFormFile? imageFile)
    {
        try
        {
            vehicle.ImageUrl = await SaveImageAsync(imageFile);
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateVehicleAsync(Vehicle vehicle, IFormFile? imageFile)
    {
        try
        {
            if (imageFile != null)
            {
                vehicle.ImageUrl = await SaveImageAsync(imageFile);
            }

            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }


    public async Task<bool> DeleteVehicleAsync(int id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);
        if (vehicle == null) return false;

        _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync();
        return true;
    }

}