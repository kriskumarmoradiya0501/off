using System.ComponentModel.DataAnnotations;

namespace examfinal.Models;

public class AdminModel
{
    public int c_adminid { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    [StringLength(150)]
    public string c_email { get; set; } = "";

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string c_password { get; set; } = "";
}
