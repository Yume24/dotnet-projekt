using System.ComponentModel.DataAnnotations;
using CarWorkshopManagementSystem.Models;

namespace CarWorkshopManagementSystem.DTOs.ServiceOrders;

public class EditServiceOrderDto
{
    public int Id { get; set; }

    [Display(Name = "Assigned Mechanic")]
    public int? MechanicId { get; set; }

    public ServiceOrderStatus Status { get; set; }

    public List<CreateServiceTaskDto> Tasks { get; set; } = new();
}
