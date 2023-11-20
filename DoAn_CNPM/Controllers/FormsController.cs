using DoAn_CNPM.Controllers;
using log4net;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DoAn_CNPM.Models
{
    public class FormsController : Controller
    {
        private DoAnCNPMEntities3 db = new DoAnCNPMEntities3();
        private static readonly ILog log = LogManager.GetLogger(typeof(FormsController));
        private int pageSize = 6;
        private int pageNumber;

        public ActionResult Index(string searchString, string searchBy, int? page)
        {
            var forms = db.Forms.Include(f => f.Patient).Include(f => f.Doctor).Include(f => f.Staff);

            pageNumber = (page ?? 1);

            if (page == null) page = 1;

            //sorting
            forms = forms.OrderBy(f => f.FormId);

            log.Info("12321321");

            if (searchBy == "All")
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    forms = forms.Where(f => f.FormId.ToString().Contains(searchString)
                    || (f.ExamTime.HasValue && f.ExamTime.Value.ToString().Contains(searchString))
                    || f.Patient.FullName.Contains(searchString)
                    || f.Doctor.FullName.Contains(searchString)
                    || f.Staff.FullName.Contains(searchString));

                }
                return View("Index", forms.ToPagedList(pageNumber, pageSize));
            }
            else if (searchBy == "Id")
            {
                return View(forms.Where(d => d.FormId.ToString().Contains(searchString) || searchString == null).ToPagedList(pageNumber, pageSize));
            }
            else if (searchBy == "ExamTime")
            {
                return View(forms.Where(d => d.ExamTime.Value.ToString().Contains(searchString) || searchString == null).ToPagedList(pageNumber, pageSize));
            }
            else if (searchBy == "Patient")
            {
                return View(forms.Where(d => d.Patient.FullName.ToString().Contains(searchString) || searchString == null).ToPagedList(pageNumber, pageSize));
            }
            else if (searchBy == "Doctor")
            {
                return View(forms.Where(d => d.Doctor.FullName.ToString().Contains(searchString) || searchString == null).ToPagedList(pageNumber, pageSize));
            }
            else if (searchBy == "Staff")
            {
                return View(forms.Where(d => d.Staff.FullName.ToString().Contains(searchString) || searchString == null).ToPagedList(pageNumber, pageSize));
            }

            return View("Index", forms.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form form = db.Forms.Find(id);
            if (form == null)
            {
                return HttpNotFound();
            }
            return View(form);
        }

        public ActionResult Create()
        {
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FullName");
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "FullName");
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FullName");
            var defaultExamTime = DateTime.Now;
            return View(new Form { ExamTime = defaultExamTime });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FormId,ExamTime,DoctorId,PatientId,StaffId,ReasonForVisit")] Form form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Forms.Add(form);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                RedirectToAction("Index", "Error");
            }

            ViewBag.PatientId = new SelectList(db.Patients, "IdPatient", "NamePatient", form.PatientId);
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "FullName", form.DoctorId);
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FullName", form.StaffId);
            return View(form);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form form = db.Forms.Find(id);
            if (form == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExamTime = form.ExamTime.Value.ToString("dddd, dd-MM-yyyy HH:mm:ss", new System.Globalization.CultureInfo("vi-VN"));
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FullName", form.PatientId);
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "FullName", form.DoctorId);
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FullName", form.StaffId);
            return View(form);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FormId,ExamTime,DoctorId,PatientId,StaffId,ReasonForVisit")] Form form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(form).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                RedirectToAction("Index", "Error");
            }
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FullName", form.PatientId);
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "FullName", form.DoctorId);
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FullName", form.StaffId);
            return View(form);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                Form form = db.Forms.Find(id);
                if (form == null)
                {
                    return HttpNotFound();
                }
                db.Forms.Remove(form);
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
