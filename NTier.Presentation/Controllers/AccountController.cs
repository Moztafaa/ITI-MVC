using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NTier.BLL.ViewModels;
using NTier.DAL.IdentityEntities;

namespace NTier.Presentation.Controllers;

[Route("[controller]/[action]")]
public class AccountController(
    UserManager<ApplicationUser> _userManager,
    SignInManager<ApplicationUser> _signInManager
) : Controller
{
    // GET: AccountController
    [HttpGet]
    public ActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
            return View(registerDto);
        }

        ApplicationUser user = new()
        {
            Email = registerDto.Email,
            PhoneNumber = registerDto.Phone,
            UserName = registerDto.Email,
            EmployeeName = registerDto.PersonName
        };
        IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction(nameof(EmployeeController.Index), "Employee");
        }
        else
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("Register", error.Description);
            }

            return View(registerDto);
        }
    }
}
