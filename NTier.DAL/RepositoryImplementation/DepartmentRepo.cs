using NTier.DAL.DataBaseContext;
using NTier.DAL.Models;
using NTier.DAL.RepositoryInterface;

namespace NTier.DAL.RepositoryImplementation;

public class DepartmentRepo(AppDbContext context) : IDepartmentRepo
{
    public IQueryable<Department> GetAllDepartmentsAsync()
    {
        var query = context.Departments.AsQueryable();
        return query;
    }

    public IQueryable<Department?> GetDepartmentByIdAsync(int id)
    {
        var query = context.Departments.Where(d => d.DepartmentId == id);
        return query;
    }

    public async Task AddDepartmentAsync(Department department)
    {
        await context.AddAsync(department);
        await context.SaveChangesAsync();
    }

    public async Task UpdateDepartmentAsync(Department department)
    {
        context.Departments.Update(department);
        await context.SaveChangesAsync();
    }

    public async Task DeleteDepartmentAsync(int id)
    {
        var department = await context.Departments.FindAsync(id);
        if (department != null)
        {
            context.Departments.Remove(department);
            await context.SaveChangesAsync();
        }
        else
        {
            throw new KeyNotFoundException($"Department with ID {id} not found.");
        }
    }
}