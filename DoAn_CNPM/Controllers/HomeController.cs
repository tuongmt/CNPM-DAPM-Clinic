using DoAn_CNPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using System.Data.Entity;
using WebGrease;

namespace DoAn_CNPM.Controllers
{
    public class HomeController : Controller
    {
        private DoAnCNPMEntities3 db = new DoAnCNPMEntities3();

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Admin_Index()
        {
            if (Session["admin"] == null)
            {
                //return RedirectToAction("Login", "LoginUser");
                return RedirectToAction("Home", "Home");
            }

            int totalForms = db.Forms.Count();
            int totalFormsOnline = db.FormOnlines.Count();
            int toatalDF = db.DetailForms.Count();
            int totalIsExamined = db.DetailForms.Count(f => f.IsExamined == true);
            int totalIsPaid = db.DetailForms.Count(f => f.IsPaid == true);
            int totalStaffs = db.Staffs.Count();
            int totalDoctors = db.Doctors.Count();
            int totalPatients = db.Patients.Count();
            var totalRevenue = db.DetailForms.Where(df => df.IsPaid == true).Sum(df => df.TotalMoney);

            ViewBag.TotalForms = totalForms;
            ViewBag.TotalFormsOnline = totalFormsOnline;
            ViewBag.TotalDF = toatalDF;
            ViewBag.ExaminedCount = totalIsExamined;
            ViewBag.PaidCount = totalIsPaid;
            ViewBag.TotalStaffs = totalStaffs;
            ViewBag.TotalDoctors = totalDoctors;
            ViewBag.TotalPatients = totalPatients;
            ViewBag.TotalRevenue = totalRevenue;

            return View();
        }

        public ActionResult FindDoctor(string searchString)
        {
            var doctors = db.Doctors.ToList();
            var doctorSchedules = db.DoctorSchedules.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                doctors = db.Doctors
                    .Where(d =>
                        d.FullName.Contains(searchString) ||
                        d.Gender.Contains(searchString) ||
                        d.Dept.DeptName.Contains(searchString)
                    )
                    .ToList();
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                doctorSchedules = db.DoctorSchedules
                    .Where(ds =>
                        ds.DayOfWeek.Contains(searchString)
                    )
                    .ToList();
            }

            ViewBag.Doctors = doctors;
            ViewBag.DoctorSchedules = doctorSchedules;
            return View();
        }

        [HttpGet]
        public ActionResult CreateFormOnline()
        {
            var d = db.Doctors.ToList();
            ViewBag.Doctors = d;
            return View();
        }
        [HttpPost]
        public ActionResult CreateFormOnline(string fullname, string gender, DateTime? DOB, string phone, 
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

        public ActionResult CreateFormOnlineSuccess()
        {
            return View();
        }

        public ActionResult MedicalExaminationPolicy()
        {
            return View();
        }
        public ActionResult CustomerPolicy()
        {
            return View();
        }
        public ActionResult ClinicPolicy()
        {
            return View();
        }

        public ActionResult CustomerCare()
        {
            return View();
        }

        public ActionResult VD5916(){
            return View();
        }

        public ActionResult VD5858()
        {
            return View();
        }


    }
}