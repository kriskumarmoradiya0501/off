using ExamMVC;
using Microsoft.AspNetCore.Mvc;

namespace ExamMVC;

public class UserController : Controller
{
    private readonly IUserInterface _userRepository;
    private readonly IDepartmentInterface _deparmentRepository;

    public UserController(
        IUserInterface userRepository,
        IDepartmentInterface departmentRepository)
    {
        _userRepository = userRepository;
        _deparmentRepository = departmentRepository;
    }

    // =========================
    // USER LIST
    // =========================
    public async Task<IActionResult> Index()
    {
        if (!IsLoggedIn())
        {
            return RedirectToAction("Login");
        }

        var users = await _userRepository.GetAll();

        return View(users);
    }

    // =========================
    // LOGIN - GET
    // =========================
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // =========================
    // LOGIN - POST
    // =========================
    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user =
            await _userRepository.Login(
                model.c_email,
                model.c_password
            );

        if (user == null)
        {
            ModelState.AddModelError(
                "",
                "Invalid email or password."
            );

            return View(model);
        }

        if (!user.c_status)
        {
            ModelState.AddModelError(
                "",
                "Your account is inactive."
            );

            return View(model);
        }

        // Store user information in Session
        HttpContext.Session.SetInt32(
            "UserId",
            user.c_userid
        );

        HttpContext.Session.SetString(
            "UserName",
            user.c_name
        );

        HttpContext.Session.SetString(
            "UserEmail",
            user.c_email
        );

        return RedirectToAction("Index");
    }

    // =========================
    // USER DETAILS
    // =========================
    public async Task<IActionResult> Details(int id)
    {
        if (!IsLoggedIn())
        {
            return RedirectToAction("Login");
        }
        var user = await _userRepository.GetById(id);

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // =========================
    // CREATE - GET
    // =========================
    [HttpGet]
    public async Task<IActionResult> Create()
    {

        if (!IsLoggedIn())
    {
        return RedirectToAction("Login");
    }
        var departments = await _deparmentRepository.GetAll();

        ViewBag.Departments = departments;

        return View();
    }

    // =========================
    // CREATE - POST
    // =========================
    [HttpPost]
    public async Task<IActionResult> Add(t_User4 user)
    {
        if (!IsLoggedIn())
    {
        return RedirectToAction("Login");
    }
        if (!ModelState.IsValid)
        {
            var departments = await _deparmentRepository.GetAll();

            ViewBag.Departments = departments;

            return View("Create", user);
        }

        // Convert hobby list into comma-separated string
        user.c_hobbies = string.Join(",", user.c_hobbyList);

        // =========================
        // IMAGE UPLOAD
        // =========================
        if (user.c_imageFile != null)
        {
            string fileName =
                Guid.NewGuid().ToString()
                + Path.GetExtension(user.c_imageFile.FileName);

            string folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images"
            );

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(
                folderPath,
                fileName
            );

            using FileStream stream =
                new FileStream(filePath, FileMode.Create);

            await user.c_imageFile.CopyToAsync(stream);

            user.c_image = fileName;
        }

        int result = await _userRepository.Add(user);

        if (result > 0)
        {
            Console.WriteLine("User added successfully");

            return RedirectToAction("Index");
        }

        return BadRequest("User was not added");
    }

    // =========================
    // EDIT - GET
    // =========================
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (!IsLoggedIn())
    {
        return RedirectToAction("Login");
    }
        var user = await _userRepository.GetById(id);

        if (user == null)
        {
            return NotFound();
        }

        // Load departments
        var departments = await _deparmentRepository.GetAll();

        ViewBag.Departments = departments;

        // Convert database hobbies:
        // Reading,Coding,Music
        //
        // into:
        // List<string>
        if (!string.IsNullOrEmpty(user.c_hobbies))
        {
            user.c_hobbyList =
                user.c_hobbies.Split(',').ToList();
        }

        // Confirm password is not stored in database.
        // Put password into confirm password for Edit form.
        user.c_confirm_password = user.c_password;

        return View(user);
    }

    // =========================
    // EDIT - POST / UPDATE
    // =========================
    [HttpPost]
    public async Task<IActionResult> Update(t_User4 user)
    {
        if (!IsLoggedIn())
    {
        return RedirectToAction("Login");
    }
        Console.WriteLine("========== UPDATE STARTED ==========");

        Console.WriteLine("User ID: " + user.c_userid);
        Console.WriteLine("Name: " + user.c_name);
        Console.WriteLine("Email: " + user.c_email);
        Console.WriteLine("Mobile: " + user.c_mobile);
        Console.WriteLine("Gender: " + user.c_gender);
        Console.WriteLine("Hobbies: " + user.c_hobbies);
        Console.WriteLine("Department: " + user.c_departmentid);
        Console.WriteLine("Age: " + user.c_age);
        Console.WriteLine("Address: " + user.c_address);
        Console.WriteLine("Status: " + user.c_status);

        if (!ModelState.IsValid)
        {
            Console.WriteLine("========== MODEL STATE INVALID ==========");

            foreach (var item in ModelState)
            {
                foreach (var error in item.Value.Errors)
                {
                    Console.WriteLine(
                        item.Key + " : " + error.ErrorMessage
                    );
                }
            }

            var departments =
                await _deparmentRepository.GetAll();

            ViewBag.Departments = departments;

            return View("Edit", user);
        }

        // Convert hobbies
        user.c_hobbies =
            string.Join(",", user.c_hobbyList);

        // =========================
        // IMAGE
        // =========================

        if (user.c_imageFile != null)
        {
            string fileName =
                Guid.NewGuid().ToString()
                + Path.GetExtension(
                    user.c_imageFile.FileName
                );

            string folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images"
            );

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath =
                Path.Combine(folderPath, fileName);

            using FileStream stream =
                new FileStream(
                    filePath,
                    FileMode.Create
                );

            await user.c_imageFile.CopyToAsync(stream);

            user.c_image = fileName;
        }

        Console.WriteLine("Calling Repository Update...");

        int result =
            await _userRepository.Update(user);

        Console.WriteLine("Update Result: " + result);

        if (result > 0)
        {
            Console.WriteLine(
                "========== UPDATE SUCCESS =========="
            );

            return RedirectToAction("Index");
        }

        Console.WriteLine(
            "========== UPDATE FAILED =========="
        );

        return BadRequest("User was not updated");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        if (!IsLoggedIn())
    {
        return RedirectToAction("Login");
    }
        var user = await _userRepository.GetById(id);

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int c_userid)
    {
        if (!IsLoggedIn())
    {
        return RedirectToAction("Login");
    }
        Console.WriteLine("========== DELETE STARTED ==========");

        Console.WriteLine("User ID received: " + c_userid);

        int result = await _userRepository.Delete(c_userid);

        Console.WriteLine("Delete Result: " + result);

        if (result > 0)
        {
            Console.WriteLine("========== DELETE SUCCESS ==========");

            return RedirectToAction("Index");
        }

        Console.WriteLine("========== DELETE FAILED ==========");

        return BadRequest("User was not deleted");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction("Login");
    }

    private bool IsLoggedIn()
    {
        return HttpContext.Session.GetInt32("UserId") != null;
    }
}
