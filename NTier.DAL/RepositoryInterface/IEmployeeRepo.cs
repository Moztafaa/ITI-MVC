using NTier.DAL.DataBaseContext;
using NTier.DAL.Models;

namespace NTier.DAL.RepositoryInterface;

public interface IEmployeeRepo
{
    // Define method signatures for employee data operations here
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task<Employee?> GetEmployeeByIdAsync(int id);
    Task AddEmployeeAsync(Employee employee);
    Task UpdateEmployeeAsync(Employee employee);
    Task DeleteEmployeeAsync(int id);
}