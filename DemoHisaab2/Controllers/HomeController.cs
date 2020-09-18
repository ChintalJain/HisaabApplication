using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoHisaab2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HttpCookie cookie = Request.Cookies["Hisaab App"];
            if(cookie!=null)
            {
                Session["UserId"] = cookie["UserId"];
                Session["UserEmail"] = cookie["UserEmail"];
                return RedirectToAction("Index", "UserHome");
            }
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}