using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarWorkshopManagementSystem.Services;
using CarWorkshopManagementSystem.DTOs.ServiceOrders;
using CarWorkshopManagementSystem.Models;

namespace CarWorkshopManagementSystem.Controllers;

[Authorize]
public class ServiceOrdersController : Controller
{
    private readonly IServiceOrderService _orderService;
    private readonly IMechanicService _mechanicService;

    public ServiceOrdersController(IServiceOrderService orderService, IMechanicService mechanicService)
    {
        _orderService = orderService;
        _mechanicService = mechanicService;
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
    [Authorize(Roles = "Admin,Receptionist")]
    public IActionResult Create()
    {
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
}
