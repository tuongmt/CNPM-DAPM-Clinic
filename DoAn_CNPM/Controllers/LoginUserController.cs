using DoAn_CNPM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn_CNPM.Controllers
{
    public class LoginUserController : Controller
    {
        private DoAnCNPMEntities3 db = new DoAnCNPMEntities3();

        // Regíter
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string Username, string Password, string ConfirmPassword, string FullName, string Gender, string Phone, DateTime? DOB)
        {
            if (ModelState.IsValid)
            {
                var check = db.Users.FirstOrDefault(u => u.Username == Username);
                if (FullName == null || Username == null || Password == null || ConfirmPassword == null)
                {
                    ViewBag.ErrorInfo = "Hãy nhập đầy đủ thông tin (*)!";
                    return View();
                }
                if (ConfirmPassword != Password)
                {
                    ViewBag.ErrorInfo = "Sai xác nhận Mật khẩu!";
                    return View();
                }
                if (check == null)
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    User user = new User
                    {
                        FullName = FullName,
                        Username = Username,
                        Password = Password,
                        ConfirmPassword = ConfirmPassword,
                        Phone = Phone,
                        RoleId = 1,
                        Gender = Gender,
                        DOB = DOB
                    };
                db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("RegisterSuccess");
                }
                else
                {
                    ViewBag.ErrorInfo = "Tên Đăng Nhập đã có rồi!";
                    return View();
                }
            }
            return View();
        }

        public ActionResult RegisterSuccess()
        {
            return View();
        }

        // GET: LoginUser
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User _user)
        {
            var acc = db.Users.SingleOrDefault(a => a.Username == _user.Username && a.Password == _user.Password);
            if (acc == null)
            {
                ViewBag.ErrorInfo = "Sai tài khoản hoặc mật khẩu";
                return View("Login");
            }
            else
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                if(acc.RoleId == 1)
                {
                    Session["patient"] = acc;
                    Session["PatientId"] = acc.UserId;
                    Session["FullName"] = acc.FullName;
                    return RedirectToAction("Patient_Index", "Users");
                }
                else if(acc.RoleId == 2)
                {
                    //Success
                    Session["admin"] = acc;
                    Session["AdminId"] = acc.UserId;
                    Session["FullName"] = acc.FullName;
                    return RedirectToAction("Admin_Index", "Home");
                }
                else
                {
                    return RedirectToAction("Home", "Home");
                }
            }
        }

        //Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Home","Home");
        }
    }
}