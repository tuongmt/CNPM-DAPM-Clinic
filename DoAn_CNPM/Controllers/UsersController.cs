using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAn_CNPM.Models;
using log4net;

namespace DoAn_CNPM.Controllers
{
    public class UsersController : Controller
    {
        private DoAnCNPMEntities3 db = new DoAnCNPMEntities3();

        private static readonly ILog log = LogManager.GetLogger(typeof(UsersController));

        private static Random random = new Random();


        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        public ActionResult Patient_Index()
        {
            return View(db.Users.ToList());
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User User = db.Users.Find(id);
            if (User == null)
            {
                return HttpNotFound();
            }
            return View(User);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,Password,FullName,Gender,Image,RoleId")] User User)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(User);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(User);
        }

        [HttpGet]
        public ActionResult CreateForm()
        {
            var d = db.Doctors.ToList();
            ViewBag.Doctors = d;
            return View(db.Users.ToList());
        }
        [HttpPost]
        public ActionResult CreateForm(string fullname, string gender, DateTime? DOB, string phone,
            string address, int doctorId, string examSession, DateTime? examDate, string reason)
        {
            FormOnline fo = new FormOnline
            {
                FullName = fullname,
                Gender = gender,
                DOB = DOB,
                Phone = phone,
                Address = address,
                DoctorId = doctorId,
                ExamSession = examSession,
                ExamDate = examDate,
                ReasonForVisit = reason
            };
            db.FormOnlines.Add(fo);
            db.SaveChanges();

            return RedirectToAction("CreateFormOnlineSuccess");
        }

        public ActionResult CreateFormSuccess()
        {
            return View();
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            if (user.Gender.ToString() == "Nam")
            {
                ViewBag.Gender = new SelectList(new List<SelectListItem>
                {
                 new SelectListItem { Text = "Nam", Value = "Nam" },
                 new SelectListItem { Text = "Nữ", Value = "Nữ" }
                }, "Value", "Text");
            }
            else
            {
                ViewBag.Gender = new SelectList(new List<SelectListItem>
                {
                 new SelectListItem { Text = "Nữ", Value = "Nữ" },
                 new SelectListItem { Text = "Nam", Value = "Nam" }

                }, "Value", "Text");
            }

            user.DOBFormatted = user.DOB?.ToString("dd-MM-yyyy");
            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "RoleName", user.RoleId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Username,Password,FullName,Gender,Image,RoleId,Phone,Address,DOB")] User user, HttpPostedFileBase uploadPhoto)
        {
            try
            {
                User user1 = db.Users.FirstOrDefault(x => x.UserId == user.UserId);
                if(user1 == null)
                {
                    log.Error("User1 không thấy nè");
                }
                user1.Password = user.Password;
                user1.FullName = user.FullName;
                user1.Gender = user.Gender;
                user1.RoleId = user.RoleId;
                user1.Phone = user.Phone;
                user1.Address = user.Address;
                user1.DOB = user.DOB;
                if (uploadPhoto != null && uploadPhoto.ContentLength > 0)
                {
                    int randomNumber = random.Next(1, 101);
                    var fileName = Path.GetFileName(randomNumber.ToString() + uploadPhoto.FileName);
                    //Tạo đường dẫn tới file
                    var path = Path.Combine(Server.MapPath("~/Images/Avatar"),fileName);
                    //Lưu tên
                    user1.Image = fileName;
                    //Save vào Images Folder
                    uploadPhoto.SaveAs(path);
                }
                db.SaveChanges();
                ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "RoleName", user.RoleId);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public ActionResult Patient_Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            if (user.Gender.ToString() == "Nam" || user.Gender == null)
            {
                ViewBag.Gender = new SelectList(new List<SelectListItem>
                {
                 new SelectListItem { Text = "Nam", Value = "Nam" },
                 new SelectListItem { Text = "Nữ", Value = "Nữ" }
                }, "Value", "Text");
            }
            else
            {
                ViewBag.Gender = new SelectList(new List<SelectListItem>
                {
                 new SelectListItem { Text = "Nữ", Value = "Nữ" },
                 new SelectListItem { Text = "Nam", Value = "Nam" }

                }, "Value", "Text");
            }

            user.DOBFormatted = user.DOB?.ToString("dd-MM-yyyy");
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Patient_Edit([Bind(Include = "UserId,Username,Password,FullName,Gender,Image,RoleId,Phone,Address,DOB")] User user, HttpPostedFileBase uploadPhoto)
        {
            try
            {
                User user1 = db.Users.FirstOrDefault(x => x.UserId == user.UserId);
                if (user1 == null)
                {
                    log.Error("User1 không thấy nè");
                }
                user1.Password = user.Password;
                user1.FullName = user.FullName;
                user1.Gender = user.Gender;
                user1.RoleId = 1;
                user1.Phone = user.Phone;
                user1.Address = user.Address;
                user1.DOB = user.DOB;
                if (uploadPhoto != null && uploadPhoto.ContentLength > 0)
                {
                    //Lấy tên file của hình được up lên
                    int randomNumber = random.Next(1, 101);
                    var fileName = Path.GetFileName(randomNumber.ToString() + uploadPhoto.FileName);
                    //Tạo đường dẫn tới file
                    var path = Path.Combine(Server.MapPath("~/Images/Avatar"), fileName);
                    //Lưu tên
                    user1.Image = fileName;
                    //Save vào Images Folder
                    uploadPhoto.SaveAs(path);
                }
                db.SaveChanges();
                ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "RoleName", user.RoleId);
                return RedirectToAction("Patient_Index");
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public ActionResult Delete(int? id)
        {
            try
            {
                User User = db.Users.Find(id);
                db.Users.Remove(User);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index","Error");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
