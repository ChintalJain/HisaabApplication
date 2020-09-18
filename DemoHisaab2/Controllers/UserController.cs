using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoHisaab2.Models;

namespace DemoHisaab2.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            ViewBag.ErrMsg = "";
            return View();
        }
        [HttpPost]
        public ActionResult Register(User_Details ud)
        {
            using(Hisaab db=new Hisaab())
            {
                User_Details ud1=db.User_Details.Where(x=>x.Email==ud.Email).FirstOrDefault();

                if(ud1!=null)
                {
                    ViewBag.ErrMsg = "User With Same Email Exist. Try With Different Email..";
                    return View();
                }
                else
                {
                    db.User_Details.Add(ud);
                    db.SaveChanges();
                    ViewBag.ErrMsg = "";
                    return RedirectToAction("Login");
                }

            }
        }
        public ActionResult Login()
        {
            ViewBag.ErrMsg = "";
            return View();
        }

        [HttpPost]
        public ActionResult Login(String Email,String Password,bool Remember)
        {
            using(Hisaab db=new Hisaab())
            {
                User_Details ud1=db.User_Details.Where(x => x.Email == Email && x.Password == Password).FirstOrDefault();
                if(ud1!=null)
                {
                    Session["UserEmail"] = ud1.Email;
                    Session["UserId"] = ud1.UserId;
                    if(Remember)
                    {
                        HttpCookie cookie = new HttpCookie("Hisaab App");
                        cookie["UserId"] = ud1.UserId.ToString();
                        cookie["UserEmail"] = ud1.Email;
                        cookie.Expires=DateTime.Now.AddDays(2);
                        Response.Cookies.Add(cookie);
                    }
                    return RedirectToAction("Index","UserHome");
                }
                else
                {
                    ViewBag.ErrMsg = "Please Enter Valid Email & Password..";
                    return View();
                }
            }
        }

        public ActionResult Logout()
        {
            Session["UserId"]= null;
            Session["UserEmail"] = null;
            HttpCookie cookie = new HttpCookie("Hisaab App");
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index","Home");
        }

        public ActionResult ViewDetails()
        {
            User_Details ud1;
            using (Hisaab db = new Hisaab())
            {
                int id = Convert.ToInt32(Session["UserId"]);
                 ud1 = db.User_Details.Where(x => x.UserId == id).FirstOrDefault();
            }
            ViewBag.Message = "";
            return View(ud1);
        }
        [HttpPost]
        public ActionResult Update(User_Details ud)
        {
            int id = Convert.ToInt32(Session["UserId"]);

            using (Hisaab db=new Hisaab())
            {
                User_Details ud1 = db.User_Details.Where(x => x.UserId == id).FirstOrDefault();
                if(ud1.Password!=ud.Password)
                {
                    ud1.Password = ud.Password;
                }
                if(ud1.FirstName!=ud.FirstName)
                {
                    ud1.FirstName = ud.FirstName;
                }
                if(ud1.LastName!=ud.LastName)
                {
                    ud1.LastName = ud.LastName;
                }
                if(ud1.ContactNumber!=ud.ContactNumber)
                {
                    ud1.ContactNumber = ud.ContactNumber;
                }
                if(ud1.DateOfBirth!=ud.DateOfBirth)
                {
                    ud1.DateOfBirth = ud.DateOfBirth;
                }
                db.SaveChanges();
            }
            ViewBag.Message = "Record Updated Successfully...";
            return RedirectToAction("ViewDetails");
        }
        
        public ActionResult Delete()
        {
            int id = Convert.ToInt32(Session["UserId"]);
            using (Hisaab db=new Hisaab())
            {
                User_Details ud1 = db.User_Details.Where(x => x.UserId ==id).FirstOrDefault();
                db.User_Details.Remove(ud1);
                db.SaveChanges();
            }
            return RedirectToAction("Index","Home");
        }
        
    }
}