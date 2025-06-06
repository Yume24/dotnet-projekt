using CarWorkshopManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
public class MechanicsController : Controller
{
    private readonly IMechanicService _service;

    public MechanicsController(IMechanicService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(string firstName, string lastName, string email, string phone, string sortBy = "Id", string sortOrder = "asc")
    {
        var mechanics = await _service.SearchMechanicsAsync(firstName, lastName, email, phone, sortBy, sortOrder);
        return View(mechanics);
    }


    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(Mechanic m)
    {
        if (!ModelState.IsValid) return View(m);

        await _service.CreateAsync(m);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var m = await _service.GetByIdAsync(id);
        if (m == null) return NotFound();

        return View(m);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Mechanic m)
    {
        if (!ModelState.IsValid) return View(m);

        await _service.UpdateAsync(m);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var m = await _service.GetByIdAsync(id);
        if (m == null) return NotFound();

        return View(m);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}