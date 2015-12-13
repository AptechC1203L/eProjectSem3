using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoctorForums.DAO;
using DoctorForums.Helpers;

namespace DoctorForums.Controllers
{
    public class AdminController : Controller
    {
        private MainDataClassDataContext dbContext;

        public AdminController()
        {
            this.dbContext = new MainDataClassDataContext();
        }
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            var loggedInUser = Session["User"] as DAO.user;

            if (loggedInUser == null || loggedInUser.role_name != "admin")
            {
                ViewBag.errorCode = "Not permited";
                ViewBag.errorMessage = "Contact to admin";
                Response.StatusCode = 404;
                return View("~/Views/ErrorPages/Index.cshtml");
            }
            else
            {
                var listUser = from u in dbContext.users
                               where u.role_name != "admin"
                               select u;                
                return View(listUser);
            }
        }

        [HttpGet] 
        public ActionResult CreateUser()
        {
            var loggedInUser = Session["User"] as DAO.user;

            if (loggedInUser == null || loggedInUser.role_name != "admin")
            {
                ViewBag.errorCode = "Not permited";
                ViewBag.errorMessage = "Contact to admin";
                Response.StatusCode = 404;
                return View("~/Views/ErrorPages/Index.cshtml");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult CreateUser(Models.DoctorViewModel model)
        {
            var loggedInUser = Session["User"] as DAO.user;

            if (loggedInUser == null || loggedInUser.role_name != "admin")
            {
                ViewBag.errorCode = "Not permited";
                ViewBag.errorMessage = "Contact to admin";
                Response.StatusCode = 404;
                return View("~/Views/ErrorPages/Index.cshtml");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var newUser = new DAO.user();

                    newUser.education = model.UserName;
                    newUser.full_name = model.FullName;
                    newUser.hash_password = SHA1.Encode(model.Password);
                    newUser.education = model.Education;
                    newUser.hospital = model.Hospital;
                    newUser.offical_location = model.OfficalLocation;
                    newUser.speciality = model.Speciality;
                    newUser.tel = model.Tel;
                    newUser.user_address = model.Address;
                    newUser.role_name = "doctor";

                    dbContext.users.InsertOnSubmit(newUser);
                    dbContext.SubmitChanges();

                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("CreateUser", "Admin");
                }
            }                        
        }

        [HttpPost]
        public ActionResult DeleteUser(int userId)
        {
            var loggedInUser = Session["User"] as DAO.user;

            if (loggedInUser == null || loggedInUser.role_name != "admin")
            {
                ViewBag.errorCode = "Not permited";
                ViewBag.errorMessage = "Contact to admin";
                Response.StatusCode = 404;
                return View("~/Views/ErrorPages/Index.cshtml");
            }
            else
            {
                var user = (from u in dbContext.users
                            where u.id == userId
                            select u).SingleOrDefault();
                if (user != null)
                {
                    if (user.is_deleted == true)
                    {
                        user.is_deleted = false;
                    }
                    else
                    {
                        user.is_deleted = true;
                    }
                }
                dbContext.SubmitChanges();
                return RedirectToAction("Index", "Admin");
            }
        }
	}
}