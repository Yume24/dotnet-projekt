namespace CarWorkshopManagementSystem.Models
{
    public class ServiceOrder
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public int? MechanicId { get; set; }
        public Mechanic? Mechanic { get; set; }
        public ServiceOrderStatus Status { get; set; } = ServiceOrderStatus.New;
        public List<ServiceTask> Tasks { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum ServiceOrderStatus
    {
        New,
        InProgress,
        Completed,
        Cancelled
    }

    public class ServiceTask
    {
        public int Id { get; set; }
        public int ServiceOrderId { get; set; }
        public ServiceOrder ServiceOrder { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal LaborCost { get; set; }
        public List<UsedPart> UsedParts { get; set; } = new();
    }

    public class UsedPart
    {
        public int Id { get; set; }
        public int ServiceTaskId { get; set; }
        public ServiceTask ServiceTask { get; set; }
        public string PartName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class Comment
    {
        public int Id { get; set; }
        public int ServiceOrderId { get; set; }
        public ServiceOrder ServiceOrder { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
