using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ProjectUmico.Web.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Name is required!")]
    [DataType(DataType.Text)]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Email is required!")]
    [DataType(DataType.EmailAddress)]
    [Remote(controller:"Authenticaiton",action:"VerifyEmail",ErrorMessage = "Email is already in use")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password is required!")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Repeat the password!")]
    [DataType(DataType.Password)]
    [Compare("Password",ErrorMessage = "Passwords should match")]
    public string RePassword { get; set; }
    
    
    public bool RememberMe { get; set; }
    public bool IsCompany { get; set; }
}