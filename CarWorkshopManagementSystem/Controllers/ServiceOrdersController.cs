using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarWorkshopManagementSystem.Services;
using CarWorkshopManagementSystem.DTOs.ServiceOrders;
using CarWorkshopManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;

namespace CarWorkshopManagementSystem.Controllers;

[Authorize]
public class ServiceOrdersController : Controller
{
    private readonly IServiceOrderService _orderService;
    private readonly IMechanicService _mechanicService;
    private readonly IVehicleService _vehicleService;

    public ServiceOrdersController(IServiceOrderService orderService, IMechanicService mechanicService, IVehicleService vehicleService)
    {
        _orderService = orderService;
        _mechanicService = mechanicService;
        _vehicleService = vehicleService;
    }

    // GET: /ServiceOrders
    public async Task<IActionResult> Index()
    {
        var orders = await _orderService.GetAllAsync();
        return View(orders);
    }

    // GET: /ServiceOrders/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
            return NotFound();

        return View(order);
    }

    // GET: /ServiceOrders/Create
    [HttpGet]
    [Authorize(Roles = "Admin,Receptionist")]
    public async Task<IActionResult> Create()
    {
        var vehicles = await _vehicleService.GetAllVehiclesAsync();
        ViewBag.Vehicles = new SelectList(vehicles, "Id", "LicensePlate");

        return View();
    }



    // POST: /ServiceOrders/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Receptionist")]
    public async Task<IActionResult> Create(CreateServiceOrderDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        await _orderService.CreateServiceOrderAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    // GET: /ServiceOrders/AssignMechanic/5
    [Authorize(Roles = "Admin,Receptionist")]
    public async Task<IActionResult> AssignMechanic(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
            return NotFound();

        var mechanics = await _mechanicService.GetAllMechanicsAsync();
        ViewBag.Mechanics = mechanics;

        return View(order);
    }

    // POST: /ServiceOrders/AssignMechanic
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Receptionist")]
    public async Task<IActionResult> AssignMechanic(int orderId, int mechanicId)
    {
        var result = await _orderService.AssignMechanicAsync(orderId, mechanicId);
        if (!result)
            return NotFound();

        return RedirectToAction(nameof(Details), new { id = orderId });
    }

    // POST: /ServiceOrders/UpdateStatus
    [HttpPost]
    [Authorize(Roles = "Admin,Mechanic")]
    public async Task<IActionResult> UpdateStatus(int orderId, ServiceOrderStatus status)
    {
        var result = await _orderService.UpdateStatusAsync(orderId, status);
        if (!result)
            return NotFound();

        return RedirectToAction(nameof(Details), new { id = orderId });
    }

    // GET: /ServiceOrders/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null) return NotFound();

        // przekszta³æ na EditServiceOrderDto
        var dto = new EditServiceOrderDto
        {
            Id = order.Id,
            MechanicId = order.MechanicId,
            Status = Enum.Parse<ServiceOrderStatus>(order.Status),
            Tasks = order.Tasks.Select(t => new CreateServiceTaskDto
            {
                Description = t.Description,
                LaborCost = t.LaborCost,
                UsedParts = t.UsedParts.Select(p => new UsedPartDto
                {
                    PartName = p.PartName,
                    Quantity = p.Quantity,
                    UnitPrice = p.UnitPrice
                }).ToList()
            }).ToList()
        };

        var mechanics = await _mechanicService.GetAllMechanicsAsync();
        ViewBag.Mechanics = new SelectList(mechanics, "Id", "LastName");

        ViewBag.Statuses = Enum.GetValues(typeof(ServiceOrderStatus))
            .Cast<ServiceOrderStatus>()
            .Select(s => new SelectListItem { Value = ((int)s).ToString(), Text = s.ToString() });

        return View(dto);
    }

    // POST: /ServiceOrders/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditServiceOrderDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        await _orderService.UpdateServiceOrderAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    // GET: /ServiceOrders/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null) return NotFound();

        return View(order); // zak³ada, ¿e masz ServiceOrderDetailsDto
    }

    // POST: /ServiceOrders/DeleteConfirmed
    [HttpPost, ActionName("DeleteConfirmed")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _orderService.DeleteServiceOrderAsync(id);
        return RedirectToAction(nameof(Index));
    }

    //PDF W DETAILS
    [Authorize]
    public async Task<IActionResult> DownloadDetailsPdf(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null) return NotFound();

        return new ViewAsPdf("Details", order)
        {
            FileName = $"ServiceOrder_{id}.pdf"
        };
    }

    [Authorize]
    public async Task<IActionResult> DownloadAllOrdersPdf()
    {
        var orders = await _orderService.GetAllAsync();

        return new ViewAsPdf("Index", orders)
        {
            FileName = $"AllServiceOrders_{DateTime.Now:yyyyMMdd}.pdf"
        };
    }


}
