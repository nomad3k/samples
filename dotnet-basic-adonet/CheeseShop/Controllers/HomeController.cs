using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CheeseShop.Domain.Members;

namespace CheeseShop.Controllers
{
    public class HomeController : Controller
    {   
        public ActionResult Index()
        {
            return View();
        }
    }
}