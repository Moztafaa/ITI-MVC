using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NTier.BLL.MappingInterface;
using NTier.BLL.ServiceInterface;
using NTier.BLL.ViewModels;
using NTier.DAL.Models;
using NTier.DAL.RepositoryInterface;

namespace NTier.BLL.ServiceImplementation;

public class EmployeeService(
    IEmployeeRepo employeeRepo,
    // IMapper<Employee, EmployeeVM> employeeMapper,
    // IMapper<CreateEmployeeVM, Employee> createEmployeeMapper,
    // IMapper<EditEmployeeVM, Employee> editEmployeeMapper
    IMapper mapper) : IEmployeeService
{
    // public async Task<IEnumerable<EmployeeVM>> GetAllEmployeesAsync()
    // {
    //     var employees = await employeeRepo.GetAllEmployeesAsync();
    //     // return employeeMapper.Map(employees);
    //     return mapper.Map<IEnumerable<EmployeeVM>>(employees);
    // }

    public async Task<IEnumerable<EmployeeVM>> GetAllEmployeesAsync()
    {
        var query = employeeRepo.GetAllEmployeesAsync();
        return await mapper.ProjectTo<EmployeeVM>(query).ToListAsync();
        // return await employeeRepo.GetAllEmployeesAsync().ProjectTo<EmployeeVM>(mapper.ConfigurationProvider)
        //     .ToListAsync();
        // return employeeMapper.Map(employees);
        // return mapper.Map<IEnumerable<EmployeeVM>>(employees);
    }

    public async Task<EmployeeVM?> GetEmployeeByIdAsync(int id)
    {
        var query = employeeRepo.GetEmployeeByIdAsync(id);

        return await mapper.ProjectTo<EmployeeVM>(query).FirstOrDefaultAsync();
        // return await employeeRepo.GetEmployeeByIdAsync(id)
        //     .ProjectTo<EmployeeVM>(mapper.ConfigurationProvider)
        //     .FirstOrDefaultAsync();

        // return employee != null ? mapper.Map<EmployeeVM>(employee) : null;
    }

    public async Task AddEmployeeAsync(CreateEmployeeVM employeeVm)
    {
        var employee = mapper.Map<Employee>(employeeVm);
        await employeeRepo.AddEmployeeAsync(employee);
    }

    public async Task UpdateEmployeeAsync(EditEmployeeVM employeeVm)
    {
        var existingEmployee = await employeeRepo.GetEmployeeByIdAsync(employeeVm.EmployeeId)
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeVm.EmployeeId);
        if (existingEmployee != null)
        {
            mapper.Map(employeeVm, existingEmployee);
            await employeeRepo.UpdateEmployeeAsync(existingEmployee);
        }
    }

    public Task DeleteEmployeeAsync(int id)
    {
        return employeeRepo.DeleteEmployeeAsync(id);
    }
}