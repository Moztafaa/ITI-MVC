using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NTier.BLL.ServiceInterface;
using NTier.BLL.ViewModels.DepartmentViewModels;
using NTier.DAL.Models;
using NTier.DAL.RepositoryInterface;

namespace NTier.BLL.ServiceImplementation;

public class DepartmentService(IDepartmentRepo departmentRepo, IMapper mapper) : IDepartmentService
{
    public async Task<IEnumerable<DepartmentVM>> GetAllDepartmentsAsync()
    {
        var departmentsQuery = departmentRepo.GetAllDepartmentsAsync();
        return await mapper.ProjectTo<DepartmentVM>(departmentsQuery).ToListAsync();
    }

    public async Task<DepartmentVM?> GetDepartmentByIdAsync(int id)
    {
        var departmentQuery = departmentRepo.GetDepartmentByIdAsync(id);
        return await mapper.ProjectTo<DepartmentVM>(departmentQuery).FirstOrDefaultAsync();
    }

    public async Task AddDepartmentAsync(CreateDepartmentVM departmentVm)
    {
        var department = mapper.Map<Department>(departmentVm);
        await departmentRepo.AddDepartmentAsync(department);
    }

    public async Task UpdateDepartmentAsync(EditDepartmentVM departmentVm)
    {
        var department = mapper.Map<Department>(departmentVm);
        await departmentRepo.UpdateDepartmentAsync(department);
    }

    public async Task DeleteDepartmentAsync(int id)
    {
        await departmentRepo.DeleteDepartmentAsync(id);
    }
}