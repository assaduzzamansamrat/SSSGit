using Services.DomainService;
using Services.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SSS.Controllers
{
    public class LoginController : Controller
    {
        LoginDomainService loginDomainService; 
        public LoginController()
        {
            loginDomainService = new LoginDomainService();
        }

        // GET: Login
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                return RedirectToAction("Index", "NewsFeed");
            }
            else
            {
                return View();
            }
           
        }

        [HttpPost]
        public ActionResult Index(LoginModel login)
        {
            try
            {
                User user;
                login.RememberMe = true;
                user = loginDomainService.AuthenticateAndGetUserByEmailAndPassword(login.UserName, login.Password);
                if(user != null)
                {
                    Session["UserName"] = user.EmailAddress;
                    Session["UserId"] = user.Id;
                    Session["Role"] = user.Role;
                    Session["WallPaper"] = user.Wallpaper;
                    Session["UserProfileImage"] = user.ImagePath;
                    Session["UserFullName"] = user.FirstName + " " + user.LastName;
                    // Set Role For Authorization
                    SetRoleForAuthorization(user, login);
                    return RedirectToAction("Index", "NewsFeed");
                }
                else
                {
                    return View("Index");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void SetRoleForAuthorization(User user,LoginModel login)
        {
            try
            {
                string userIdAndRoleKeyValuePair = user.Id.ToString() + ";" + user.Role;
                var authTicket = new FormsAuthenticationTicket(
                               1,                             // version
                               user.EmailAddress,             // user name
                               DateTime.Now,                  // created
                               DateTime.Now.AddDays(365),     // expires
                               login.RememberMe,              // persistent?
                               userIdAndRoleKeyValuePair      // can be used to store roles
                               );

                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);

            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public ActionResult LogOut()
        {
            long userId = (long)Session["UserId"];
            loginDomainService.UpdateIsUserLoggedInField(userId, false);
            Session["UserName"] = null;
            Session["UserId"] = null;
            Session["Role"] = null;
            Session["WallPaper"] = null;
            Session["UserProfileImage"] = null;
            Session["UserFullName"] = null;
            return RedirectToAction("Index", "Login");
        }
    }
}