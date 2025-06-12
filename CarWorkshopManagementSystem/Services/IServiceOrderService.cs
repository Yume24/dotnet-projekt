using CarWorkshopManagementSystem.Models;
using CarWorkshopManagementSystem.DTOs.ServiceOrders;

namespace CarWorkshopManagementSystem.Services;




public interface IServiceOrderService
{
    Task<int> CreateServiceOrderAsync(CreateServiceOrderDto dto);
    Task<ServiceOrderDetailsDto?> GetOrderByIdAsync(int id);
    Task<List<ServiceOrderDetailsDto>> GetAllAsync();
    Task<bool> AssignMechanicAsync(int orderId, int mechanicId);
    Task<bool> UpdateStatusAsync(int orderId, ServiceOrderStatus status);
    Task UpdateServiceOrderAsync(EditServiceOrderDto dto);
    Task DeleteServiceOrderAsync(int id);
    Task<byte[]> GenerateOrderPdfAsync(int id);
    Task<byte[]> GenerateAllOrdersPdfAsync();

}
