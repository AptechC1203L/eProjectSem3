using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DoctorForums.DAO;
using DoctorForums.Helpers;

namespace DoctorForums.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Your email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Telephone")]
        public string Tel { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

    }
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember on this computer")]
        public bool RememberMe { get; set; }
        public string FullName { get; set; }

        /// <summary>
        /// Checks if user with given password exists in the database
        /// </summary>
        /// <param name="_username">User name</param>
        /// <param name="_password">User password</param>
        /// <returns>True if user exist and password is correct</returns>
        public bool IsValid(string _username, string _password)
        {
            MainDataClassDataContext db = new MainDataClassDataContext();
            String _hashPassword = SHA1.Encode(_password);
            var record = from u in db.users 
                       where u.email.ToString() == _username
                       && u.hash_password.ToString() == _hashPassword
                       select u;
            var user = record.SingleOrDefault();           
            
            if (user == null || user.is_deleted == true)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }
    public class DoctorViewModel
    {
        [Required]
        [Display(Name = "Your email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [Required]
        [StringLength(12, ErrorMessage = "This is not phone number")]
        [Display(Name = "Telephone")]
        public string Tel { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        
        [Display(Name = "Speciality")]
        public string Speciality { get; set; }
        [Display(Name = "Offical Location")]
        public string OfficalLocation { get; set; }
        [Display(Name = "Education")]
        public string Education { get; set; }
        [Display(Name = "Hospital")]
        public string Hospital { get; set; }

    }
}