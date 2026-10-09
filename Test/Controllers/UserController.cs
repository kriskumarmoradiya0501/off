using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Test.Models;
using Test.Repository.Services;

namespace Test.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly precticeServices _Service;

        public UserController(ILogger<UserController> logger, precticeServices Service)
        {
            _Service = Service;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAll()
        {
            var users = _Service.GetAll();

            return Json(new
            {
                success = true,
                data = users
            });
        }
        
        [HttpGet]
        public async Task<IActionResult> GetbyId(int id)
        {
            var user = await _Service.GetUser(id);

            return Json(new
            {
                success = true,
                data = user
            });
        }

        [HttpPost]
    public async Task<IActionResult> Save(User user)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                message = "Invalid data"
            });
        }

        var result = await _Service.Save(user);

        return Json(new
        {
            success = result,
            message = result
                ? "User saved successfully"
                : "Unable to save user"
        });
    }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _Service.DeteleUser(id);

            return Json (new
            {
                success = true,
                message = "Deleted successfully"
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}