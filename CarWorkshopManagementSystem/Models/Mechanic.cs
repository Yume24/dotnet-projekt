using System.ComponentModel.DataAnnotations;

namespace CarWorkshopManagementSystem.Models;

public class Mechanic
{
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string Email { get; set; }

    public string? PhoneNumber { get; set; }


}