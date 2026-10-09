using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Test.Models
{
    public class vm_Login
    {
        [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
    }
}