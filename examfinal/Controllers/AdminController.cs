using examfinal.Models;
using examfinal.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace examfinal.Controllers;

public class AdminController : Controller
{
    private readonly ExamRepository _repository;

    public AdminController(ExamRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (HttpContext.Session.GetInt32("AdminId") != null)
        {
            return RedirectToAction("Index", "User");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(AdminModel admin)
    {
        if (!ModelState.IsValid)
        {
            return View(admin);
        }

        if (!await _repository.RegisterAdmin(admin))
        {
            ModelState.AddModelError(nameof(admin.c_email), "This email is already registered.");
            return View(admin);
        }

        TempData["Message"] = "Registration successful. Please log in.";
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (HttpContext.Session.GetInt32("AdminId") != null)
        {
            return RedirectToAction("Index", "User");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var admin = await _repository.LoginAdmin(model.c_email, model.c_password);
        if (admin == null)
        {
            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

        HttpContext.Session.SetInt32("AdminId", admin.c_adminid);
        HttpContext.Session.SetString("AdminEmail", admin.c_email);

        return RedirectToAction("Index", "User");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }
}
