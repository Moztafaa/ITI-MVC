using NTier.BLL.ViewModels;

namespace NTier.BLL.ServiceInterface;

public interface IEmployeeService
{
    // Define service methods related to Employee operations here
    Task<IEnumerable<EmployeeVM>> GetAllEmployeesAsync();
    Task<EmployeeVM?> GetEmployeeByIdAsync(int id);
    Task AddEmployeeAsync(CreateEmployeeVM employeeVm);
    Task UpdateEmployeeAsync(EditEmployeeVM employeeVm);
    Task DeleteEmployeeAsync(int id);
}