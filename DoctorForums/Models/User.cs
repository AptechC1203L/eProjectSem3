using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DoctorForums.DAO;

namespace DoctorForums.Models
{
    public class User
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
 
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
 
        [Display(Name = "Remember on this computer")]
        public bool RememberMe { get; set; }
        
        /// <summary>
        /// Checks if user with given password exists in the database
        /// </summary>
        /// <param name="_username">User name</param>
        /// <param name="_password">User password</param>
        /// <returns>True if user exist and password is correct</returns>
        public bool IsValid(string _username, string _password)
        {
            MainDataClassDataContext db = new MainDataClassDataContext();
            var user = from u in db.users where u.email == _username select u.hash_password;
            String password = user.SingleOrDefault();
            if (password.CompareTo(_password) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        
        }
    }
}