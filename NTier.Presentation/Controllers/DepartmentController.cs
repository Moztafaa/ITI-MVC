using Microsoft.AspNetCore.Mvc;
using NTier.BLL.ServiceInterface;
using NTier.BLL.ViewModels.DepartmentViewModels;

namespace NTier.Presentation.Controllers;

public class DepartmentController(IDepartmentService departmentService) : Controller
{
    // GET: Department/Index
    public async Task<IActionResult> Index()
    {
        var departments = await departmentService.GetAllDepartmentsAsync();
        return View(departments);
    }

    // GET: Department/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Department/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateDepartmentVM departmentVm)
    {
        if (!ModelState.IsValid)
        {
            return View(departmentVm);
        }

        await departmentService.AddDepartmentAsync(departmentVm);
        return RedirectToAction(nameof(Index));
    }

    // GET: Department/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var department = await departmentService.GetDepartmentByIdAsync(id);
        if (department == null)
        {
            return NotFound();
        }

        var editDepartmentVm = new EditDepartmentVM
        {
            DepartmentId = department.DepartmentId,
            Name = department.Name
        };

        return View(editDepartmentVm);
    }

    // POST: Department/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditDepartmentVM departmentVm)
    {
        if (!ModelState.IsValid)
        {
            return View(departmentVm);
        }

        await departmentService.UpdateDepartmentAsync(departmentVm);
        return RedirectToAction(nameof(Index));
    }

    // GET: Department/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var department = await departmentService.GetDepartmentByIdAsync(id);
        if (department == null)
        {
            return NotFound();
        }

        return View(department);
    }

    // POST: Department/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await departmentService.DeleteDepartmentAsync(id);
        return RedirectToAction(nameof(Index));
    }
}