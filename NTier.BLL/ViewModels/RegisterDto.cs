using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NTier.BLL.ViewModels;

public class RegisterDto
{
    [Required(ErrorMessage = "Name can't be blank")]
    public required string PersonName { get; set; }


    [Required(ErrorMessage = "Email can't be blank")]
    [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
    [Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "Email is already in use")] // Remote validation (ViewFeatures Nuget Package)
    public required string Email { get; set; }


    [Required(ErrorMessage = "Phone can't be blank")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number should contain numbers only")]
    [DataType(DataType.PhoneNumber)]
    public required string Phone { get; set; }


    [Required(ErrorMessage = "Password can't be blank")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }


    [Required(ErrorMessage = "Confirm Password can't be blank")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
    public required string ConfirmPassword { get; set; }

}
