using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DoctorForums.DAO;
using DoctorForums.Helpers;
using System.ComponentModel.DataAnnotations;

namespace DoctorForums.Controllers
{
    public class UserController : Controller
    {
        public static int LoggedInCount { get; set; }

        private MainDataClassDataContext dbContext;

        public static int GetUserCount()
        {
            var db = new DAO.MainDataClassDataContext();
            return db.users.Count();
        }

        public UserController()
        {
            dbContext = new DAO.MainDataClassDataContext();
        }

        public ActionResult Details(int id)
        {
            // FIXME: Show an error page if no such user is found
            // FIXME: Show a proper error page if the profile is private
            var loggedInUser = Session["User"] as DAO.user;
            var user = dbContext.users.SingleOrDefault(u => u.id == id);

            if (loggedInUser == null)
            {
                ViewBag.errorCode = "Unauthorized";
                ViewBag.errorMessage = "Please login to view this user's profile.";
                Response.StatusCode = 401;
                return View("~/Views/ErrorPages/Index.cshtml");
            }
            else if (user == null)
            {
                ViewBag.errorCode = "Not found";
                ViewBag.errorMessage = "There is no such profile on this system.";
                Response.StatusCode = 404;
                return View("~/Views/ErrorPages/Index.cshtml");
            }
            else if (user.is_private == true && user.id != loggedInUser.id)
            {
                ViewBag.errorCode = "Forbidden";
                ViewBag.errorMessage = "This user wants their profile to be private.";
                Response.StatusCode = 403;
                return View("~/Views/ErrorPages/Index.cshtml");
            }
            else
            {
                return View(user);
            }
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
        public ActionResult Login(Models.LoginViewModel user, string next)
        {
            if (ModelState.IsValid)
            {
                if (user.IsValid(user.UserName, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, user.RememberMe);
                    var sessionUser = from u in dbContext.users where u.email == user.UserName select u;
                    Session["User"] = sessionUser.SingleOrDefault();
                    LoggedInCount++;

                    if (next != null)
                        return Redirect(next);
                    else
                        return RedirectToAction("Index", "Rooms");
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

                TempData["message"] = "Welcome! you can post question now";
                Session["User"] = newUser;
                LoggedInCount++;

                return RedirectToAction("Index", "Rooms");
            }

            return View(user);
        }

        public ActionResult Logout(string next)
        {
            FormsAuthentication.SignOut();
            System.Web.HttpContext.Current.Session.Remove("User");
            LoggedInCount--;

            if (next != null)
                return Redirect(next);
            else
                return RedirectToAction("Index", "Rooms");
        }

        [HttpGet]
        public ActionResult ViewNotification(int id)
        {
            var user = Session["User"] as DAO.user;

            var noti = user.notifications.Where(n => n.id == id).SingleOrDefault();
            noti.is_read = true;

            dbContext.SubmitChanges();

            return Redirect(noti.url);
        }

        [HttpPost]
        public ActionResult TogglePrivacy(bool isPrivate)
        {
            var userId = (Session["User"] as DAO.user).id;
            var user = dbContext.users.SingleOrDefault(u => u.id == userId);
            user.is_private = isPrivate;
            this.dbContext.SubmitChanges();

            return RedirectToAction("Details", new { id = user.id });
        }

        public class UserSearchViewModel
        {
            [Display(Name = "Find doctors with name containing...")]
            public string Name { get; set; }

            public IEnumerable<DAO.user> SearchResults;
        }

        [HttpGet]
        public ActionResult Search()
        {
            return View(new UserSearchViewModel { SearchResults = new List<user>()});
        }

        [HttpPost]
        public ActionResult Search(UserSearchViewModel searchData)
        {
            searchData.SearchResults = dbContext.users
                .Where(user => searchData.Name != null && user.full_name.Contains(searchData.Name));
            return View(searchData);
        }
	}
}