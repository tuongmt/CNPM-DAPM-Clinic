using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAn_CNPM.Models;
using PagedList;

namespace DoAn_CNPM.Controllers
{
    public class FormOnlinesController : Controller
    {
        private DoAnCNPMEntities3 db = new DoAnCNPMEntities3();
        private int pageSize = 6;
        private int pageNumber;

        public ActionResult Index(string searchString, string searchBy, int? page)
        {
            var formOnlines = db.FormOnlines.Include(fo => fo.Doctor);
            pageNumber = (page ?? 1);
            if (page == null) page = 1;
            //sorting
            formOnlines = formOnlines.OrderBy(d => d.FOId);

            if (searchBy == "All")
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    formOnlines = formOnlines.Where(fo => fo.FOId.ToString().Contains(searchString)
                    || fo.FullName.Contains(searchString)
                    || fo.Gender.ToString().Contains(searchString)
                    || fo.Phone.Contains(searchString)
                    || fo.Doctor.FullName.ToString().Contains(searchString)
                    || fo.ReasonForVisit.Contains(searchString));
                }
                return View(formOnlines.ToPagedList(pageNumber, pageSize));

            }
            else if (searchBy == "Name")
            {
                return View(formOnlines.Where(d => d.FullName.ToString().Contains(searchString) || searchString == null).ToPagedList(pageNumber, pageSize));
            }
            else if (searchBy == "Gender")
            {
                return View(formOnlines.Where(d => d.Gender.ToString().Contains(searchString) || searchString == null).ToPagedList(pageNumber, pageSize));
            }
            else if (searchBy == "PhoneNumber")
            {
                return View(formOnlines.Where(d => d.Phone.ToString().Contains(searchString) || searchString == null).ToPagedList(pageNumber, pageSize));
            }
            else if (searchBy == "Doctor")
            {
                return View(formOnlines.Where(d => d.Doctor.FullName.ToString().Contains(searchString) || searchString == null).ToPagedList(pageNumber, pageSize));
            }
            else if (searchBy == "Reason")
            {
                return View(formOnlines.Where(d => d.ReasonForVisit.ToString().Contains(searchString) || searchString == null).ToPagedList(pageNumber, pageSize));
            }

            
            return View(formOnlines.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormOnline formOnline = db.FormOnlines.Find(id);
            if (formOnline == null)
            {
                return HttpNotFound();
            }
            return View(formOnline);
        }

        public ActionResult Create()
        {
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "FullName");
            ViewBag.Gender = new SelectList(new List<SelectListItem>
            {
               new SelectListItem { Text = "Nam", Value = "Nam" },
               new SelectListItem { Text = "Nữ", Value = "Nữ" }
            }, "Value", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FullName,Gender,Phone,DOB,Address,DoctorId,ReasonForVisit")] FormOnline formOnline)
        {
            if (ModelState.IsValid)
            {
                db.FormOnlines.Add(formOnline);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "FullName", formOnline.DoctorId);
            ViewBag.Gender = new SelectList(formOnline.Gender);
            return View(formOnline);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormOnline formOnline = db.FormOnlines.Find(id);
            if (formOnline == null)
            {
                return HttpNotFound();
            }

            if(formOnline.Gender.ToString() == "Nam")
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

            formOnline.DOBFormatted = formOnline.DOB?.ToString("dd-MM-yyyy");

            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "FullName", formOnline.DoctorId);
            return View(formOnline);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FOId,FullName,Gender,Phone,DOB,Address,DoctorId,CreatedDay,ReasonForVisit,ExamSession,ExamDate")] FormOnline formOnline)
        {
            if (ModelState.IsValid)
            {
                db.Entry(formOnline).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Gender = new SelectList(formOnline.Gender);
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "FullName", formOnline.DoctorId);
            return View(formOnline);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                FormOnline formOnline = db.FormOnlines.Find(id);
                db.FormOnlines.Remove(formOnline);
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
