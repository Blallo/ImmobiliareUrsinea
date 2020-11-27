using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AgenziaMVC.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace AgenziaMVC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [AllowAnonymous]
        public ActionResult Login()
        {
           return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = System.Web.Configuration.WebConfigurationManager.AppSettings["LoginUserName"];
            var pwd = System.Web.Configuration.WebConfigurationManager.AppSettings["LoginPwd"];
         
            if (model.Name.Equals(user) && model.Password.Equals(pwd))
            {
                if (model.RememberMe)
                {
                    var authTicket = new FormsAuthenticationTicket(1, model.Name, DateTime.Now, DateTime.Now.AddDays(3), true, "", "/");
                    //encrypt the ticket and add it to a cookie
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                    Response.Cookies.Add(cookie);
                }
                return RedirectToAction("Immobili", "Immobili");
            }
            else
            {
                return View();
            }
        }
    }
}