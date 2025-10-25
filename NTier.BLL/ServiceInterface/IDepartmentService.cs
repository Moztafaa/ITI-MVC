using NTier.BLL.ViewModels.DepartmentViewModels;

namespace NTier.BLL.ServiceInterface;

public interface IDepartmentService
{
    // Define department-related service methods here
    Task<IEnumerable<DepartmentVM>> GetAllDepartmentsAsync();
    Task<DepartmentVM?> GetDepartmentByIdAsync(int id);
    Task AddDepartmentAsync(CreateDepartmentVM departmentVm);
    Task UpdateDepartmentAsync(EditDepartmentVM departmentVm);
    Task DeleteDepartmentAsync(int id);
}