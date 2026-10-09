using System.ComponentModel.DataAnnotations;

namespace ExamMVC;

public class t_User4
{
    public int c_userid { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [Display(Name = "Name")]
    public string c_name { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "Invalid email formate.")]
    public string c_email { get; set; }

    [Required(ErrorMessage = "Mobile is required")]
    [Display(Name = "Mobile")]
    public string c_mobile { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string c_password { get; set; }

    [Required(ErrorMessage = "Cofirm password is required.")]
    [Compare("c_password", ErrorMessage = "Passwods do not match.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string c_confirm_password { get; set; }

    [Required(ErrorMessage = "Gender is Required")]
    [Display(Name = "Gender")]
    public string c_gender { get; set; }

    [Display(Name = "Hobbies")]
    public string? c_hobbies { get; set; }
    public List<string> c_hobbyList { get; set; } = new List<string>();

    [Required(ErrorMessage = "Department is required")]
    [Display(Name = "Department")]
    public int c_departmentid { get; set; }

    [Required(ErrorMessage = "Date of birth is required")]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateOnly c_dob { get; set; }

    [Range(18, 60, ErrorMessage = "Age must be between 18 and 60")]
    [Display(Name = "Age")]
    public int? c_age { get; set; }

    [Display(Name = "Address")]
    public string? c_address { get; set; }

    [Display(Name = "Image")]
    public string? c_image { get; set; }
    public IFormFile? c_imageFile { get; set; }

    [Display(Name = "Status")]
    public bool c_status { get; set; }

}
