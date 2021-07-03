using Services.DomainService;
using Services.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SSS.Controllers
{
    public class SignUpController : Controller
    {
        UserDomainService userDomainService;
        public SignUpController()
        {
             userDomainService = new UserDomainService();
        }
        
        // GET: SignUp
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(User user)
        {
            bool isUserCreated = false;
            try
            {
                
                isUserCreated = userDomainService.CreateNewUser(user);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}