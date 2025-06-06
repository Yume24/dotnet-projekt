using CarWorkshopManagementSystem.Data;
using CarWorkshopManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

public class MechanicService : IMechanicService
{
    private readonly ApplicationDbContext _context;

    public MechanicService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Mechanic>> GetAllAsync()
    {
        return await _context.Mechanics.ToListAsync();
    }

    public async Task<Mechanic?> GetByIdAsync(int id)
    {
        return await _context.Mechanics.FindAsync(id);
    }

    public async Task<bool> CreateAsync(Mechanic mechanic)
    {
        _context.Mechanics.Add(mechanic);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(Mechanic mechanic)
    {
        _context.Mechanics.Update(mechanic);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var m = await _context.Mechanics.FindAsync(id);
        if (m == null) return false;

        _context.Mechanics.Remove(m);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<List<Mechanic>> SearchMechanicsAsync(string firstName, string lastName, string email, string phone, string sortBy, string sortOrder)
    {
        var query = _context.Mechanics.AsQueryable();

        if (!string.IsNullOrWhiteSpace(firstName))
            query = query.Where(m => m.FirstName.Contains(firstName));
        if (!string.IsNullOrWhiteSpace(lastName))
            query = query.Where(m => m.LastName.Contains(lastName));
        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(m => m.Email.Contains(email));
        if (!string.IsNullOrWhiteSpace(phone))
            query = query.Where(m => m.PhoneNumber.Contains(phone));

        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            if (sortOrder == "desc")
                query = query.OrderByDescending(m => EF.Property<object>(m, sortBy));
            else
                query = query.OrderBy(m => EF.Property<object>(m, sortBy));
        }

        return await query.ToListAsync();
    }

    public async Task<List<Mechanic>> GetAllMechanicsAsync()
    {
        return await _context.Mechanics.ToListAsync();
    }

}