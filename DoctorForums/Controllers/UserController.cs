using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DoctorForums.Helpers;

namespace DoctorForums.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (user.IsValid(user.UserName, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, user.RememberMe);
                    var dbContext = new DAO.MainDataClassDataContext();
                    var sessionUser = from u in dbContext.users where u.email == user.UserName select u;
                    System.Web.HttpContext.Current.Session["user_name"] = sessionUser.SingleOrDefault().email;
                    System.Web.HttpContext.Current.Session["user_full_name"] = sessionUser.SingleOrDefault().full_name;
                    //Fix me!
                    System.Web.HttpContext.Current.Session["token"] = sessionUser.SingleOrDefault().hash_password;
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult Register(Models.RegisterViewModel user)
        {
            if (ModelState.IsValid)
            {
                DAO.user newUser = new DAO.user();
                newUser.email = user.UserName;
                newUser.full_name = user.FullName;
                newUser.hash_password = SHA1.Encode(user.Password);
                newUser.tel = user.Tel;
                newUser.role_name = "none";

                DAO.MainDataClassDataContext db = new DAO.MainDataClassDataContext();
                db.users.InsertOnSubmit(newUser);
                
                db.SubmitChanges();

                ViewBag.Message = "Welcome! you can login and post question now";
                return View("~/Views/Home/Index.cshtml");
            }
            return View(user);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            System.Web.HttpContext.Current.Session.Remove("user_name");
            return RedirectToAction("Index", "Home");
        }
	}
}