using CarWorkshopManagementSystem.Data;
using CarWorkshopManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using CarWorkshopManagementSystem.DTOs.ServiceOrders;
using CarWorkshopManagementSystem.PdfReports;


namespace CarWorkshopManagementSystem.Services;




public class ServiceOrderService : IServiceOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ServiceOrderService> _logger;

    public ServiceOrderService(ApplicationDbContext context, ILogger<ServiceOrderService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> CreateServiceOrderAsync(CreateServiceOrderDto dto)
    {
        var order = new ServiceOrder
        {
            VehicleId = dto.VehicleId,
            Tasks = dto.Tasks.Select(t => new ServiceTask
            {
                Description = t.Description,
                LaborCost = t.LaborCost,
                UsedParts = t.UsedParts.Select(p => new UsedPart
                {
                    PartName = p.PartName,
                    Quantity = p.Quantity,
                    UnitPrice = p.UnitPrice
                }).ToList()
            }).ToList()
        };

        _context.ServiceOrders.Add(order);
        await _context.SaveChangesAsync();
        return order.Id;
    }

    public async Task<ServiceOrderDetailsDto?> GetOrderByIdAsync(int id)
    {
        try
        {
            return await _context.ServiceOrders
            .Include(o => o.Vehicle)
            .Include(o => o.Mechanic)
            .Include(o => o.Tasks).ThenInclude(t => t.UsedParts)
            .Include(o => o.Comments)
            .Where(o => o.Id == id)
            .Select(o => new ServiceOrderDetailsDto
            {
                Id = o.Id,
                Status = o.Status.ToString(),
                CreatedAt = o.CreatedAt,
                Vehicle = $"{o.Vehicle.Brand} {o.Vehicle.Model} ({o.Vehicle.LicensePlate})",
                Mechanic = o.Mechanic != null ? $"{o.Mechanic.FirstName} {o.Mechanic.LastName}" : null,
                MechanicId = o.MechanicId,
                Tasks = o.Tasks.Select(t => new ServiceTaskDetailsDto
                {
                    Description = t.Description,
                    LaborCost = t.LaborCost,
                    UsedParts = t.UsedParts.Select(p => new UsedPartDto
                    {
                        PartName = p.PartName,
                        Quantity = p.Quantity,
                        UnitPrice = p.UnitPrice
                    }).ToList()
                }).ToList(),
                Comments = o.Comments.Select(c => new CommentDto
                {
                    Author = c.Author,
                    Content = c.Content,
                    Timestamp = c.Timestamp
                }).ToList()
            }).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"B��d przy pobieraniu zlecenia ID: {id}");
            throw;
        }
    }

    public async Task<List<ServiceOrderDetailsDto>> GetAllAsync()
    {
        return await _context.ServiceOrders
            .Include(o => o.Vehicle)
            .Include(o => o.Mechanic)
            .Select(o => new ServiceOrderDetailsDto
            {
                Id = o.Id,
                Status = o.Status.ToString(),
                CreatedAt = o.CreatedAt,
                Vehicle = $"{o.Vehicle.Brand} {o.Vehicle.Model} ({o.Vehicle.LicensePlate})",
                Mechanic = o.Mechanic != null ? $"{o.Mechanic.FirstName} {o.Mechanic.LastName}" : null
            }).ToListAsync();
    }

    public async Task<bool> AssignMechanicAsync(int orderId, int mechanicId)
    {
        var order = await _context.ServiceOrders.FindAsync(orderId);
        if (order == null) return false;
        order.MechanicId = mechanicId;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateStatusAsync(int orderId, ServiceOrderStatus status)
    {
        var order = await _context.ServiceOrders.FindAsync(orderId);
        if (order == null) return false;
        order.Status = status;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task UpdateServiceOrderAsync(EditServiceOrderDto dto)
    {
        var order = await _context.ServiceOrders
            .Include(o => o.Tasks)
            .ThenInclude(t => t.UsedParts)
            .FirstOrDefaultAsync(o => o.Id == dto.Id);

        if (order == null) return;

        order.MechanicId = dto.MechanicId;
        order.Status = dto.Status;

        // Wyczy�� stare dane
        _context.ServiceTasks.RemoveRange(order.Tasks);

        // Dodaj nowe zadania
        order.Tasks = dto.Tasks.Select(t => new ServiceTask
        {
            Description = t.Description,
            LaborCost = t.LaborCost,
            UsedParts = t.UsedParts.Select(p => new UsedPart
            {
                PartName = p.PartName,
                Quantity = p.Quantity,
                UnitPrice = p.UnitPrice
            }).ToList()
        }).ToList();

        await _context.SaveChangesAsync();
    }


    public async Task DeleteServiceOrderAsync(int id)
    {
        var order = await _context.ServiceOrders
            .Include(o => o.Tasks)
            .ThenInclude(t => t.UsedParts)
            .Include(o => o.Comments)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return;

        _context.Comments.RemoveRange(order.Comments);
        foreach (var task in order.Tasks)
        {
            _context.UsedParts.RemoveRange(task.UsedParts);
        }
        _context.ServiceTasks.RemoveRange(order.Tasks);
        _context.ServiceOrders.Remove(order);

        await _context.SaveChangesAsync();
    }

    public async Task<byte[]> GenerateOrderPdfAsync(int id)
    {
        var order = await GetOrderByIdAsync(id);
        if (order == null)
            throw new InvalidOperationException($"Zlecenie {id} nie istnieje.");

        return ServiceOrderPdfGenerator.Generate(order);
    }
    public async Task<byte[]> GenerateAllOrdersPdfAsync()
    {
        var orders = await GetAllAsync();
        return AllOrdersPdfGenerator.Generate(orders);
    }

}
