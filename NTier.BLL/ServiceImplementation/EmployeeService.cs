using NTier.BLL.MappingInterface;
using NTier.BLL.ServiceInterface;
using NTier.BLL.ViewModels;
using NTier.DAL.Models;
using NTier.DAL.RepositoryInterface;

namespace NTier.BLL.ServiceImplementation;

public class EmployeeService(
    IEmployeeRepo employeeRepo,
    IMapper<Employee, EmployeeVM> employeeMapper,
    IMapper<CreateEmployeeVM, Employee> createEmployeeMapper,
    IMapper<EditEmployeeVM, Employee> editEmployeeMapper
) : IEmployeeService
{
    public async Task<IEnumerable<EmployeeVM>> GetAllEmployeesAsync()
    {
        var employees = await employeeRepo.GetAllEmployeesAsync();
        return employeeMapper.Map(employees);
    }

    public async Task<EmployeeVM?> GetEmployeeByIdAsync(int id)
    {
        var employee = await employeeRepo.GetEmployeeByIdAsync(id);
        return employee != null ? employeeMapper.Map(employee) : null;
    }

    public async Task AddEmployeeAsync(CreateEmployeeVM employeeVm)
    {
        var employee = createEmployeeMapper.Map(employeeVm);
        await employeeRepo.AddEmployeeAsync(employee);
    }

    public async Task UpdateEmployeeAsync(EditEmployeeVM employeeVm)
    {
        var existingEmployee = await employeeRepo.GetEmployeeByIdAsync(employeeVm.EmployeeId);
        if (existingEmployee != null)
        {
            // Use generic mapper to update only matching properties; nulls are ignored
            editEmployeeMapper.MapToExisting(employeeVm, existingEmployee);
            await employeeRepo.UpdateEmployeeAsync(existingEmployee);
        }
    }

    public Task DeleteEmployeeAsync(int id)
    {
        return employeeRepo.DeleteEmployeeAsync(id);
    }
}