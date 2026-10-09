using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Test.Models;
using Test.Repository.Services;
using Test.Repository;
using Microsoft.AspNetCore.Identity;


namespace Test.Controllers;


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly RegisterService _registerService;

    public HomeController(ILogger<HomeController> logger, RegisterService registerService)
    {
        _registerService = registerService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(User user)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Please fill all fields.");
            return View(user);
        }
            await _registerService.RegisterUser(user);
            return RedirectToAction("Login");
    }
    public IActionResult Login()
    {
        return View();
    }
    public IActionResult Admin()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(vm_Login user)
    {
        if(!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Invalid login attempt.");
        return View(user);
        }
        var existingUser = await _registerService.LoginUser(user);

        if (existingUser)
        {
            HttpContext.Session.SetString("Email",user.Email);
            HttpContext.Session.SetString ("Role","user");
            return RedirectToAction("Index","User");
        }

        return View(user);
        
    }
    [HttpPost]
    public async Task<IActionResult> Admin(vm_Login user)
    {
        var existingUser = await _registerService.LoginAdmin(user);

        if (existingUser)
        {
            HttpContext.Session.SetString("Email", user.Email);
            HttpContext.Session.SetString("Role", "Admin");

            HttpContext.Session.SetString ("Role", "admin");
            return RedirectToAction("Index","Admin");
        }

        ModelState.AddModelError("", "Invalid login attempt.");
        return View(user);
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
