using NTier.DAL.Models;

namespace NTier.BLL.ViewModels.DepartmentViewModels;

public class DepartmentVM
{
    public int DepartmentId { get; set; }
    public string Name { get; set; }

    public ICollection<Employee> Employees { get; set; }
}