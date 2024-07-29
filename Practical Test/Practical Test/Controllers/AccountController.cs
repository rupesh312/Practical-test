using Practical_Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Practical_Test.Controllers
{
    public class AccountController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
       
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.Username == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
                    System.Diagnostics.Debug.WriteLine("User authenticated: " + model.Username);
                    // Check if the user is an admin
                    if (user.Role == "Admin")
                    {
                        return RedirectToAction("Index", "Users");
                    }

                    // Redirect to the return URL or default
                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError("", "The username or password provided is incorrect.");
            }

            return View(model);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}