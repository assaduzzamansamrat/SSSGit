using Services.DomainService;
using Services.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSS.Controllers
{
    public class HomeController : Controller
    {
        UserDomainService userDomainService = new UserDomainService(); 

        
        // GET: Home
        public ActionResult Index()
        {
            try
            {
                List<User> usersList = new List<User>();
                usersList = userDomainService.GetAllUsersData();
                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }
    }
}