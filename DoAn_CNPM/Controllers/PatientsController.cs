using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAn_CNPM.Models;

namespace DoAn_CNPM.Controllers
{
    public class PatientsController : Controller
    {
        private DoAnCNPMEntities3 db = new DoAnCNPMEntities3();      

        public ActionResult Index(string SearchString)
        {
            if (!String.IsNullOrEmpty(SearchString))
            {
                var pats = db.Patients.Where(p => p.PatientId.ToString().Contains(SearchString)
                || p.FullName.Contains(SearchString)
                || p.Gender.Contains(SearchString)
                || p.Phone.Contains(SearchString)
                || p.DOB.ToString().Contains(SearchString)
                || p.Address.Contains(SearchString));
                return View(pats.ToList());
            }
            return View(db.Patients.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        public ActionResult Create()
        {
            ViewBag.Gender = new SelectList(new List<SelectListItem>
            {
               new SelectListItem { Text = "Nam", Value = "Nam" },
               new SelectListItem { Text = "Nữ", Value = "Nữ" }
            }, "Value", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientId,FullName,Gender,Phone,DOB,Address")] Patient patient)
        {
            try {
                if (ModelState.IsValid)
                {
                    db.Patients.Add(patient);
                    db.SaveChanges();
                    
                }
                ViewBag.Gender = new SelectList(patient.Gender);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index","Error");
            }
        }

        [HttpGet]
        public ActionResult CreateForm()
        {
            var d = db.Doctors.ToList();
            ViewBag.Doctors = d;
            return View();
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

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Patient patient = db.Patients.Find(id);

            if (patient == null)
            {
                return HttpNotFound();
            }

            if (patient.Gender.ToString() == "Nam")
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


            patient.DOBFormatted = patient.DOB?.ToString("dd-MM-yyyy");

            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatientId,FullName,Gender,Phone,DOB,Address")] Patient patient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(patient).State = EntityState.Modified;
                    db.SaveChanges();                 
                }
                ViewBag.Gender = new SelectList(patient.Gender);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                Patient patient = db.Patients.Find(id);
                db.Patients.Remove(patient);
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
