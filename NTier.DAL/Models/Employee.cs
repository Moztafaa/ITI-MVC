using System.ComponentModel.DataAnnotations;

namespace NTier.DAL.Models;

public class Employee
{
    [Key] public int EmployeeId { get; set; }
    public string Name { get; set; }
    public string? Gender { get; set; }

    public decimal Salary { get; set; }

    public string Address { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; }
}
