using System.ComponentModel.DataAnnotations;

namespace NTier.DAL.Models;

public class Department
{
    [Key] public int DepartmentId { get; set; }
    public string Name { get; set; }

    public ICollection<Employee> Employees { get; set; }
}