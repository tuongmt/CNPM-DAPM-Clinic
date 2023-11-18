using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DoAn_CNPM.Models
{
    public class FormsController : Controller
    {
        private DoAnCNPMEntities3 db = new DoAnCNPMEntities3();

        public ActionResult Index(string SearchString)
        {
            var forms = db.Forms.Include(f => f.Patient).Include(f => f.Doctor).Include(f => f.Staff);
            if (!String.IsNullOrEmpty(SearchString))
            {
                forms = forms.Where(f => f.FormId.ToString().Contains(SearchString) 
                || f.ExamTime.ToString().Contains(SearchString)
                || f.Patient.FullName.Contains(SearchString)
                || f.Doctor.FullName.Contains(SearchString)
                || f.Staff.FullName.Contains(SearchString));
            }
            return View(forms.ToList());
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
