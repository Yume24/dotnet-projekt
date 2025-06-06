namespace CarWorkshopManagementSystem.DTOs.ServiceOrders;
public class CreateServiceOrderDto
{
    public int VehicleId { get; set; }
    public List<CreateServiceTaskDto> Tasks { get; set; } = new();
}

public class CreateServiceTaskDto
{
    public string Description { get; set; } = string.Empty;
    public decimal LaborCost { get; set; }
    public List<UsedPartDto> UsedParts { get; set; } = new();
}

public class UsedPartDto
{
    public string PartName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class ServiceOrderDetailsDto
{
    public int Id { get; set; }
    public string Status { get; set; }
    public string Vehicle { get; set; }
    public string? Mechanic { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ServiceTaskDetailsDto> Tasks { get; set; } = new();
    public List<CommentDto> Comments { get; set; } = new();
}

public class ServiceTaskDetailsDto
{
    public string Description { get; set; }
    public decimal LaborCost { get; set; }
    public List<UsedPartDto> UsedParts { get; set; } = new();
}

public class CommentDto
{
    public string Author { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
}
