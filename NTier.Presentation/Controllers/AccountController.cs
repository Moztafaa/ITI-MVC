using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NTier.BLL.Enums;
using NTier.BLL.ViewModels;
using NTier.DAL.IdentityEntities;

namespace NTier.Presentation.Controllers;

[Route("[controller]/[action]")]
[AllowAnonymous]
public class AccountController(
    UserManager<ApplicationUser> _userManager,
    SignInManager<ApplicationUser> _signInManager,
    RoleManager<ApplicationRole> _roleManager
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
            if (registerDto.UserType == BLL.Enums.UserTypesOptions.Admin)
            {
                // create admin role if it doesn't exist
                if (await _roleManager.FindByNameAsync(UserTypesOptions.Admin.ToString()) is null)
                {
                    ApplicationRole applicationRole = new()
                    {
                        Name = UserTypesOptions.Admin.ToString()
                    };
                    await _roleManager.CreateAsync(applicationRole);
                }
                // assign user to Admin role
                await _userManager.AddToRoleAsync(user, UserTypesOptions.Admin.ToString());
            }
            else
            {
                // assign user to User role

                if (await _roleManager.FindByNameAsync(UserTypesOptions.User.ToString()) is null)
                {
                    ApplicationRole applicationRole = new()
                    {
                        Name = UserTypesOptions.User.ToString()
                    };
                    await _roleManager.CreateAsync(applicationRole);
                }
                await _userManager.AddToRoleAsync(user, UserTypesOptions.User.ToString());
            }
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
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginVM loginVM, string? ReturnUrl)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
            return View(loginVM);
        }

        var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, false, false);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return LocalRedirect(ReturnUrl);
            }
            return RedirectToAction("Index", "Employee");
        }
        ModelState.AddModelError("Login", "Invalid Login Attempt");
        return View(loginVM);
    }
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }

    public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return Json(true);
        }
        else
        {
            return Json(false);
        }
    }


}
