using Microsoft.EntityFrameworkCore;
using NTier.DAL.DataBaseContext;
using NTier.DAL.Models;
using NTier.DAL.RepositoryInterface;

namespace NTier.DAL.RepositoryImplementation;

public class EmployeeRepo(AppDbContext context) : IEmployeeRepo
{
    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        return await context.Employees.Include(e => e.Department).ToListAsync();
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        return await context.Employees.Include(e => e.Department).FirstOrDefaultAsync(e => e.EmployeeId == id);
    }

    public async Task AddEmployeeAsync(Employee employee)
    {
        await context.AddAsync(employee);
        await context.SaveChangesAsync();
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {
        context.Employees.Update(employee);
        await context.SaveChangesAsync();
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        var employee = await context.Employees.FindAsync(id);
        if (employee != null)
        {
            context.Employees.Remove(employee);
            await context.SaveChangesAsync();
        }
        else
        {
            throw new KeyNotFoundException($"Employee with ID {id} not found.");
        }
    }
}