using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NTier.DAL.IdentityEntities;

namespace NTier.Presentation.Controllers;

public class RoleController(RoleManager<ApplicationRole> roleManager) : Controller
{


    // GET: RoleController
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    public async Task<IActionResult> Create(string roleName)
    {
        try
        {
            ApplicationRole? getRoleByName = await roleManager.FindByNameAsync(roleName);
            if (getRoleByName != null)
            {
                ViewBag.Message = "Role already exists.";
                return View();
            }
            else
            {
                ApplicationRole applicationRole = new()
                {
                    Name = roleName
                };
                IdentityResult result = await roleManager.CreateAsync(applicationRole);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Role created successfully.";
                    return View();
                }
                else
                {
                    ViewBag.Message = "Error creating role.";
                    return View();
                }
            }
        }
        catch
        {
            ViewBag.Message = "An error occurred.";
            return View();
        }
    }

}