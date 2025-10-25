using NTier.DAL.Models;

namespace NTier.DAL.RepositoryInterface;

public interface IDepartmentRepo
{
    // Define method signatures for department data operations here with mapping projection for create operation
    IQueryable<Department> GetAllDepartmentsAsync();
    IQueryable<Department?> GetDepartmentByIdAsync(int id);
    Task AddDepartmentAsync(Department department);
    Task UpdateDepartmentAsync(Department department);
    Task DeleteDepartmentAsync(int id);
}