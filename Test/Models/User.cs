using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
//add validation attribute
namespace Test.Models
{
    public class User
    {
        [Key]
        public int Id {get; set;}
        [Required(ErrorMessage ="Name is required")]
        public string? Name {get; set;}
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage ="Invalid Email format")]
        public string? Email {get; set;}
        [Required(ErrorMessage ="Password is required")]
        //[RegularExpression(ErrorMessage ="correct password")]
        public string? Password {get; set;}

        [Compare("Password")]
        public string? ConfimPassword {get; set;}

    }
}