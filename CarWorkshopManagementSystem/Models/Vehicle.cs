using System.ComponentModel.DataAnnotations;

namespace CarWorkshopManagementSystem.Models;

public class Vehicle
{
    public int Id { get; set; }
    [Required]
    public string Brand { get; set; }
    [Required]
    public string Model { get; set; }
    [Required]
    public int Year { get; set; }
    [Required]
    public string LicensePlate { get; set; }
    [Required]
    public int OwnerId { get; set; }
}