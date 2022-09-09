using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectUmico.Web.Models;

public class LoginViewModel
{
    [EmailAddress]
    public string Email { get; set; }

    [PasswordPropertyText]
    public string Password { get; set; }
    
    public bool RememberMe { get; set; }
    public bool IsCompany { get; set; }
    
}