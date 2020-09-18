using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoHisaab2.Models;
using System.Globalization;

namespace DemoHisaab2.Controllers
{
    public class UserHomeController : Controller
    {
        // GET: UserHome
        Hisaab db = new Hisaab();

        public ActionResult Index()
        {
            
            if(Session["UserId"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            return RedirectToAction("DisplayTransaction");
        }
        [AllowAnonymous]
        public ActionResult AccountDetails()
        {
            var result = checkSession();
            if (result != null)
                return result;
            string[] items = { "Bank", "Simple", "Bill" };
            ViewData["AccountType"] = new SelectList(items.AsEnumerable<string>());
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult AccountDetails(string InitAmountType,User_Accounts usracc)
        {
            var result = checkSession();
            if (result != null)
                return result;
            int id = Convert.ToInt32(Session["UserId"]);
            if(InitAmountType.Equals("Debit"))
            {
                usracc.TotalAmount = 0 - usracc.TotalAmount;
            }

            using(Hisaab dbx=new Hisaab())
            {
                usracc.UserId = id;
                usracc.User_Details = dbx.User_Details.Find(id);
                dbx.User_Accounts.Add(usracc);
                Transaction trax = new Transaction();
                trax.CreditAccount = usracc.AccountName;
                trax.Amount = usracc.TotalAmount;
                trax.DebitAccount = "";
                trax.Description = "Intialization";
                trax.Date = DateTime.Now;
                trax.UserId = id;
                trax.User_Details= dbx.User_Details.Find(id);
                dbx.Transactions.Add(trax);
                dbx.SaveChanges();
            }
            return RedirectToAction("AccountDetails");
        }

        public ActionResult Transaction()
        {
            var result = checkSession();
            if (result != null)
                return result;
            int id = Convert.ToInt32(Session["UserId"]);
            var usr = db.User_Accounts.Where(p => p.UserId == id);
            var accounts = usr.Select(acc => acc.AccountName);
            ViewData["CreditAccount"] = new SelectList(accounts.AsEnumerable<string>());
            ViewData["DebitAccount"] = new SelectList(accounts.AsEnumerable<string>());
            return View();
        }
        public ActionResult Transaction1()
        {
            var result = checkSession();
            if (result != null)
                return result;
            return View();
        }
        [HttpPost]
        public ActionResult Transaction1(string PreDefinedAcc)
        {
            var result = checkSession();
            if (result != null)
                return result;

            if (PreDefinedAcc.Equals("Yes"))
                return RedirectToAction("Transaction");
            else
                return (RedirectToAction("Transaction2"));
        }
        public ActionResult Transaction2()
        {
            var result = checkSession();
            if (result != null)
                return result;

            int id = Convert.ToInt32(Session["UserId"]);
            var usr = db.User_Accounts.Where(p => p.UserId == id);
            var accounts = usr.Select(acc => acc.AccountName);
            ViewData["CreditAccount"] = new SelectList(accounts.AsEnumerable<string>());
            return View();
        }
        [HttpPost]
        public ActionResult Transaction2(Transaction ts)
        {
            var result = checkSession();
            if (result != null)
                return result;

            int id = Convert.ToInt32(Session["UserId"]);
            User_Accounts usracc = db.User_Accounts.Where(x => x.AccountName.Equals(ts.CreditAccount)).FirstOrDefault();
            usracc.TotalAmount += ts.Amount;
            ts.UserId = id;
            ts.User_Details = db.User_Details.Find(id);

            db.Transactions.Add(ts);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Transaction(Transaction ts)
        {
            var result = checkSession();
            if (result != null)
                return result;

            int id = Convert.ToInt32(Session["UserId"]);
            User_Accounts usracc = db.User_Accounts.Where(x => x.AccountName.Equals(ts.CreditAccount)).FirstOrDefault();
            usracc.TotalAmount += ts.Amount;
            usracc = db.User_Accounts.Where(x => x.AccountName.Equals(ts.DebitAccount)).FirstOrDefault();
            usracc.TotalAmount -= ts.Amount;
            ts.UserId = id;
            ts.User_Details = db.User_Details.Find(id);
            db.Transactions.Add(ts);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult DisplayTransaction()
        {
            var result=checkSession();
            if (result != null)
                return result;

            int id = Convert.ToInt32(Session["UserId"]);
            var ts = db.Transactions.Where(t => t.UserId == id).OrderByDescending(x=>x.Date);
            return View(ts);
        }
        public ActionResult ShowAccountTransaction(string AccountName)
        {
            var result = checkSession();
            if (result != null)
                return result;

            int id = Convert.ToInt32(Session["UserId"]);
            var trs = db.Transactions.Where(ac => ac.UserId == id && (ac.CreditAccount == AccountName || ac.DebitAccount == AccountName)).OrderByDescending(x=>x.Date);
            ViewData["accname"] = AccountName;
            return View(trs);
        }
        public ActionResult DispAccounts()
        {
            var result = checkSession();
            if (result != null)
                return result;

            int id = Convert.ToInt32(Session["UserId"]);
            var acc = db.User_Accounts.Where(ac => ac.UserId == id);
            return View(acc);
        }
        public ActionResult DeleteTransaction(int id,string AccountName)
        {
            var result = checkSession();
            if (result != null)
                return result;
            var trs = db.Transactions.Where(ts => ts.TransactionId == id).FirstOrDefault();
            db.Transactions.Remove(trs);
            db.SaveChanges();
            var usracc = db.User_Accounts.Where(ua => ua.AccountName == trs.CreditAccount).FirstOrDefault();
            usracc.TotalAmount -= trs.Amount;
            User_Accounts usac = db.User_Accounts.Where(u => u.AccountName == trs.DebitAccount).FirstOrDefault();
            if (usac != null)
            {
                usac.TotalAmount += trs.Amount;
            }
            db.SaveChanges();
            return RedirectToAction("ShowAccountTransaction",new { AccountName = AccountName});
        }
        public ActionResult DeleteTransaction1(int id)
        {
            var result = checkSession();
            if (result != null)
                return result;

            var trs = db.Transactions.Where(ts => ts.TransactionId == id).FirstOrDefault();
            db.Transactions.Remove(trs);
            db.SaveChanges();
            var usracc = db.User_Accounts.Where(ua => ua.AccountName == trs.CreditAccount).FirstOrDefault();
            usracc.TotalAmount -= trs.Amount;
            User_Accounts usac = db.User_Accounts.Where(u => u.AccountName == trs.DebitAccount).FirstOrDefault();
            if (usac != null)
            {
                usac.TotalAmount += trs.Amount;
            }
            db.SaveChanges();
            return RedirectToAction("DisplayTransaction");
        }

        public ActionResult EditTransaction(int id)
        {
            var result = checkSession();
            if (result != null)
                return result;

            var trs = db.Transactions.Where(ts => ts.TransactionId == id).FirstOrDefault();
            ViewBag.usrId = trs.UserId;
            return View(trs);
        }
        [HttpPost]
        public ActionResult EditTransaction(Transaction ts)
        {
            var result = checkSession();
            if (result != null)
                return result;

            Transaction trs = db.Transactions.Where(t => t.TransactionId == ts.TransactionId).FirstOrDefault();
            if(trs.Date!=ts.Date)
            {
                trs.Date = ts.Date;
            }
            if(!trs.Description.Equals(ts))
            {
                trs.Description = ts.Description;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private ActionResult checkSession()
        {
            int id = Convert.ToInt32(Session["UserId"]);
            string name = Convert.ToString(Session["UserName"]);
            if(id==0 || name==null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return null;
            }
        }

        public ActionResult Filter(string StartDate, string EndDate)
        {
            DateTime StDate = Convert.ToDateTime(DateTime.ParseExact(StartDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
            DateTime EdDate = Convert.ToDateTime(DateTime.ParseExact(EndDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
            int id = Convert.ToInt32(Session["UserId"]);
            IEnumerable<Transaction> ts = db.Transactions.Where(t => t.UserId == id && (t.Date <= EdDate && t.Date >= StDate)).OrderByDescending(x=>x.Date).ToList();
            return View("DisplayTransaction",ts);
        }


    }
}