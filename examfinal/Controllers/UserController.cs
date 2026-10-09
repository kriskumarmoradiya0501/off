using examfinal.Models;
using examfinal.Repositories;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace examfinal.Controllers;

public class UserController : Controller
{
    private readonly ExamRepository _repository;

    public UserController(ExamRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (!IsLoggedIn())
        {
            return RedirectToAction("Login", "Admin");
        }

        ViewBag.Departments = await _repository.GetDepartments();
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!IsLoggedIn())
        {
            return Unauthorized(new { success = false, message = "Please log in again." });
        }

        var users = await _repository.GetUsers(HttpContext.Session.GetInt32("AdminId")!.Value);
        return Json(new { success = true, data = users });
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        if (!IsLoggedIn())
        {
            return Unauthorized(new { success = false, message = "Please log in again." });
        }

        var user = await _repository.GetUser(id, HttpContext.Session.GetInt32("AdminId")!.Value);
        if (user == null)
        {
            return NotFound(new { success = false, message = "User not found." });
        }

        return Json(new { success = true, data = user });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(UserModel user)
    {
        if (!IsLoggedIn())
        {
            return Unauthorized(new { success = false, message = "Please log in again." });
        }

        if (!ModelState.IsValid)
        {
            var message = string.Join(
                " ",
                ModelState.Values.SelectMany(value => value.Errors)
                    .Select(error => error.ErrorMessage));

            return BadRequest(new { success = false, message });
        }

        var adminId = HttpContext.Session.GetInt32("AdminId")!.Value;
        int result;

        try
        {
            result = user.c_userid == 0
                ? await _repository.AddUser(user, adminId)
                : await _repository.UpdateUser(user, adminId);
        }
        catch (PostgresException exception) when (
            exception.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            return Conflict(new { success = false, message = "This email is already registered." });
        }
        catch (PostgresException exception) when (
            exception.SqlState == PostgresErrorCodes.ForeignKeyViolation)
        {
            return BadRequest(new { success = false, message = "Select a valid department." });
        }

        if (result == 0)
        {
            return NotFound(new { success = false, message = "User was not found." });
        }

        return Json(new
        {
            success = true,
            message = user.c_userid == 0 ? "User added successfully." : "User updated successfully."
        });
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (!IsLoggedIn())
        {
            return Unauthorized(new { success = false, message = "Please log in again." });
        }

        var result = await _repository.DeleteUser(
            id,
            HttpContext.Session.GetInt32("AdminId")!.Value);

        if (result == 0)
        {
            return NotFound(new { success = false, message = "User was not found." });
        }

        return Json(new { success = true, message = "User deleted successfully." });
    }

    private bool IsLoggedIn()
    {
        return HttpContext.Session.GetInt32("AdminId") != null;
    }
}
