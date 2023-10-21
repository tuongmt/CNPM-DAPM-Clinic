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

        // GET: Forms
        public ActionResult Index(string SearchString)
        {
            var forms = db.Forms.Include(f => f.Patient).Include(f => f.Doctor).Include(f => f.Staff);
            if (!String.IsNullOrEmpty(SearchString))
            {
                forms = forms.Where(f => f.FormId.ToString().Contains(SearchString) 
                || f.Patient.FullName.Contains(SearchString)
                || f.Doctor.FullName.Contains(SearchString)
                || f.Staff.FullName.Contains(SearchString));
            }
            return View(forms.ToList());
        }

        // GET: Forms/Details/5
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

        // GET: Forms/Create
        public ActionResult Create()
        {
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FullName");
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "FullName");
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FullName");
            var defaultExamTime = DateTime.Now;
            return View(new Form { ExamTime = defaultExamTime });
        }

        // POST: Forms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FormId,ExamTime,DoctorId,PatientId,StaffId,ReasonForVisit")] Form form)
        {
            if (ModelState.IsValid)
            {
                db.Forms.Add(form);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PatientId = new SelectList(db.Patients, "IdPatient", "NamePatient", form.PatientId);
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "FullName", form.DoctorId);
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FullName", form.StaffId);
            return View(form);
        }

        // GET: Forms/Edit/5
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
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FullName", form.PatientId);
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "FullName", form.DoctorId);
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FullName", form.StaffId);
            return View(form);
        }

        // POST: Forms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FormId,ExamTime,DoctorId,PatientId,StaffId,ReasonForVisit")] Form form)
        {
            if (ModelState.IsValid)
            {
                db.Entry(form).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FullName", form.PatientId);
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "FullName", form.DoctorId);
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FullName", form.StaffId);
            return View(form);
        }

        // GET: Forms/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Forms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
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
