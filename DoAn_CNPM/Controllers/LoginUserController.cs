using DoAn_CNPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn_CNPM.Controllers
{
    public class LoginUserController : Controller
    {
        private DoAnCNPMEntities3 db = new DoAnCNPMEntities3();
        // GET: LoginUser
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var usr = username;
            var psw = password;
            var acc = db.AdminUsers.SingleOrDefault(a => a.Username == username && a.Password == password);
            if (acc != null)
            {
                //Success
                Session["admin"] = acc;
                Session["name"] = acc.NameAdminUser;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorInfo = "Sai thông tin, hãy xem xét lại";
                return View("Login");
            }
        }
        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }
    }
}