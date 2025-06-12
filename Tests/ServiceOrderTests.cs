using Xunit;
using CarWorkshopManagementSystem.Models;
using System;

namespace Tests;

public class ServiceOrderTests
{
    [Fact]
    public void CreateServiceOrder_ShouldHaveDefaultStatusAndDate()
    {
        var order = new ServiceOrder
        {
            VehicleId = 1
        };

        Assert.Equal(ServiceOrderStatus.New, order.Status);
        Assert.True((DateTime.UtcNow - order.CreatedAt).TotalSeconds < 10);
    }

    [Fact]
    public void AddTaskToOrder_ShouldIncreaseTaskCount()
    {
        var order = new ServiceOrder();
        order.Tasks.Add(new ServiceTask { Description = "Wymiana oleju", LaborCost = 100 });

        Assert.Single(order.Tasks);
        Assert.Equal("Wymiana oleju", order.Tasks[0].Description);
    }
}