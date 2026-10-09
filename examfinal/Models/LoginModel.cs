using System.ComponentModel.DataAnnotations;

namespace examfinal.Models;

public class LoginModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string c_email { get; set; } = "";

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string c_password { get; set; } = "";
}
