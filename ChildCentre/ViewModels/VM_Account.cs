using ChildCentre.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChildCentre.ViewModels
{

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class ProfileModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First name")]
        public String GivenName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public String LastName { get; set; }

        [Required]
        [Display(Name = "Email Name")]
        public String Email { get; set; }

        [Display(Name = "Primary Number")]
        public String PrimaryPhone { get; set; }

        [Display(Name = "Secondary Number")]
        public String SecondaryPhone { get; set; }

        public Address Address { get; set; }

    }

    public class StaffModel : ProfileModel
    {
        public string StaffType { get; set; }
    }

    public class RegisterModel : ProfileModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class List
    {
        public int Id { get; set; }
        public String UserName { get; set; }
        public String LastName { get; set; }
        public String GivenName { get; set; }
        public String Email { get; set; }
        public String StaffType { get; set; }
    }
    public class StaffList : List 
    {
        public String StaffType { get; set; }
    }

    public class GuardianList : List
    {
        public String GuardianType { get; set; }
    }
}