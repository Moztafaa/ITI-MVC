using Microsoft.AspNetCore.Mvc;
using NTier.BLL.ServiceInterface;
using NTier.BLL.ViewModels;

namespace NTier.Presentation.Controllers;

public class EmployeeController(IEmployeeService employeeService) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var employees = await employeeService.GetAllEmployeesAsync();
        return View(employees);
    }

    public async Task<IActionResult> Details(int id)
    {
        EmployeeVM? employee = await employeeService.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        return View(employee);
    }

    public async Task<IActionResult> Create()
    {
        return View(new CreateEmployeeVM());
    }

    public async Task<IActionResult> CreateEmployee(CreateEmployeeVM employeeVm)
    {
        if (ModelState.IsValid)
        {
            await employeeService.AddEmployeeAsync(employeeVm);
            return RedirectToAction("Index");
        }

        return View("Create");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var employee = await employeeService.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            return RedirectToAction("Index");
        }

        EditEmployeeVM editEmployeeVm = new()
        {
            EmployeeId = employee.EmployeeId,
            Name = employee.Name,
            DepartmentId = employee.DepartmentId
        };
        return View(editEmployeeVm);
    }

    public async Task<IActionResult> EditEmployee(EditEmployeeVM employeeVm)
    {
        if (ModelState.IsValid)
        {
            await employeeService.UpdateEmployeeAsync(employeeVm);
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeleteEmployee(int id)
    {
        await employeeService.DeleteEmployeeAsync(id);
        return RedirectToAction("Index");
    }
}