using System.ComponentModel.DataAnnotations;

namespace examfinal.Models;

public class UserModel
{
    public int c_userid { get; set; }

    public int c_adminid { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100)]
    public string c_name { get; set; } = "";

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    [StringLength(150)]
    public string c_email { get; set; } = "";

    [Required(ErrorMessage = "Mobile is required.")]
    [StringLength(15)]
    public string c_mobile { get; set; } = "";

    [Required(ErrorMessage = "Gender is required.")]
    [StringLength(20)]
    public string c_gender { get; set; } = "";

    [StringLength(200)]
    public string? c_hobbies { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Select a department.")]
    public int c_departmentid { get; set; }

    [Required(ErrorMessage = "Date of birth is required.")]
    public DateOnly? c_dob { get; set; }

    [Range(18, 60, ErrorMessage = "Age must be between 18 and 60.")]
    public int? c_age { get; set; }

    [StringLength(500)]
    public string? c_address { get; set; }

    [StringLength(200)]
    public string? c_image { get; set; }

    public bool c_status { get; set; } = true;

    public string c_departmentname { get; set; } = "";
}
