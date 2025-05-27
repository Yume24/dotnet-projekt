using System.ComponentModel.DataAnnotations;

namespace CarWorkshopManagementSystem.Models;

public class Client
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
}