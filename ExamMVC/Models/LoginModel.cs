using System.ComponentModel.DataAnnotations;

namespace ExamMVC;

public class LoginModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string c_email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string c_password { get; set; }
}