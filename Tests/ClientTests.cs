using Xunit;
using CarWorkshopManagementSystem.Models;

namespace Tests;

public class ClientTests
{
    [Fact]
    public void CreateClient_WithValidData_ShouldBeValid()
    {
        var client = new Client
        {
            Name = "Anna",
            Surname = "Nowak",
            Email = "anna@example.com",
            PhoneNumber = "123456789"
        };

        Assert.NotNull(client);
        Assert.Equal("Anna", client.Name);
        Assert.Equal("Nowak", client.Surname);
    }
}