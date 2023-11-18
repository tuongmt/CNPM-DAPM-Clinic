using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DoAn_CNPM.Models
{
    public class DoctorsController : Controller
    {
        private DoAnCNPMEntities3 db = new DoAnCNPMEntities3();

        public ActionResult Index(string SearchString)
        {
            if (!String.IsNullOrEmpty(SearchString))
            {
                var doctor = db.Doctors.Where(d => d.DoctorId.ToString().Contains(SearchString)
                || d.FullName.Contains(SearchString)
                || d.Gender.Contains(SearchString)
                || d.Phone.Contains(SearchString)
                || d.Dept.DeptName.Contains(SearchString));
                return View(doctor.ToList());
            }
            return View(db.Doctors.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        public ActionResult Create()
        {
            ViewBag.Gender = new SelectList(new List<SelectListItem>
            {
               new SelectListItem { Text = "Nam", Value = "Nam" },
               new SelectListItem { Text = "Nữ", Value = "Nữ" }
            }, "Value", "Text");

            ViewBag.DeptId = new SelectList(db.Depts, "DeptId", "DeptName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DoctorId,FullName,Gender,Phone,DeptId,Image")] Doctor doctor, HttpPostedFileBase uploadPhoto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Doctors.Add(doctor);
                    db.SaveChanges();
                    if (uploadPhoto != null && uploadPhoto.ContentLength > 0)
                    {
                        int id = int.Parse(db.Doctors.ToList().Last().DoctorId.ToString());
                        string fileName = "";
                        int index = uploadPhoto.FileName.IndexOf('.');
                        fileName = "doctor" + id.ToString() + "." + uploadPhoto.FileName.Substring(index + 1);
                        string path = Path.Combine(Server.MapPath("~/Images/Doctor"), fileName);
                        uploadPhoto.SaveAs(path);

                        Doctor doctor1 = db.Doctors.FirstOrDefault(x => x.DoctorId == id);
                        doctor1.Image = fileName;
                        db.SaveChanges();
                    }
                }
                ViewBag.Gender = new SelectList(doctor.Gender);
                ViewBag.DeptId = new SelectList(db.Depts, "DeptId", "DeptName", doctor.DeptId);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index","Error");
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Doctor doctor = db.Doctors.Find(id);

            if (doctor == null)
            {
                return HttpNotFound();
            }

            if (doctor.Gender.ToString() == "Nam")
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

            ViewBag.DeptId = new SelectList(db.Depts, "DeptId", "DeptName", doctor.DeptId);

            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DoctorId,FullName,Gender,Phone,DeptId,Image")] Doctor doctor, HttpPostedFileBase uploadPhoto)
        {
            try
            {
                Doctor doctor1 = db.Doctors.FirstOrDefault(x => x.DoctorId == doctor.DoctorId);
                doctor1.FullName = doctor.FullName;
                doctor1.Gender = doctor.Gender;
                doctor1.Phone = doctor.Phone;
                doctor1.DeptId = doctor.DeptId;
                if (uploadPhoto != null && uploadPhoto.ContentLength > 0)
                {
                    int id = doctor.DoctorId;
                    string fileName = "";
                    int index = uploadPhoto.FileName.IndexOf('.');
                    fileName = "doctor" + id.ToString() + "." + uploadPhoto.FileName.Substring(index + 1);
                    string path = Path.Combine(Server.MapPath("~/Images/Doctor"), fileName);
                    uploadPhoto.SaveAs(path);
                    doctor1.Image = fileName;
                }
                db.SaveChanges();
                ViewBag.DeptId = new SelectList(db.Depts, "DeptId", "DeptName", doctor.DeptId);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index","Error");
            }
            
        }

        public ActionResult Delete(int id)
        {
            try
            {
                Doctor doctor = db.Doctors.Find(id);
                db.Doctors.Remove(doctor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index", "Error");
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
