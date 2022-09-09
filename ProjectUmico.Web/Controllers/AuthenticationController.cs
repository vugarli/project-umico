using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Web.Models;
using umico.Models;

namespace ProjectUmico.Web.Controllers;

[Route("[controller]/[action]")]
public class AuthenticationController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthenticationController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        //TODO action filter
        if (User.Identity is {IsAuthenticated: true})
        {
            return RedirectToAction("Index", "Home");
        }
        
        return View();
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity is {IsAuthenticated: true})
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                return View();
            }
        }

        ModelState.AddModelError("", "Login failed");
        return View();
    }
    

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        ApplicationUser user;
        if (model.IsCompany)
        {
            user = new Company()
            {
                Email = model.Email,
                UserName = model.Name,
                CreatedAt = DateTime.Now,
            };
        }
        else
        {
            user = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Name,
                CreatedAt = DateTime.Now,
            };
        }

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user,false);
            return View();
        }
        else
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View();
    }

    
    public async Task<JsonResult> VerifyEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return Json(data: true);
        }

        return Json(data: "Already registered Email");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}