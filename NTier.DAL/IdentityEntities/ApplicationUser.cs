using System;
using Microsoft.AspNetCore.Identity;

namespace NTier.DAL.IdentityEntities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? EmployeeName { get; set; }
}
