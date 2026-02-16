using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using CollegeApp.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Options;
namespace CollegeApp.Models
{
    public class StudentDTO
    {
        [ValidateNever]
        public int Id {get;set;}
         [Required(ErrorMessage = "Student name is required")]
         [StringLength(30)]
        public string StudentName {get;set;} = "";
         [EmailAddress]
        public string Email {get;set;}  ="";

        // [Range(10,20)]
        // public int Age {get;set;}
        // [Required]
        public string Address {get;set;} = "";

        public DateTime DOB {get;set;}

        // public string ? Password {get;set;}
        // [Compare("Password")]
        // public string? ConfirmPassword {get;set;}
        // [DateCheck]
        // public DateTime AdmissionDate {get;set;}
    }
}