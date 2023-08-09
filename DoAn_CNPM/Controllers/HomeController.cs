using DoAn_CNPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn_CNPM.Controllers
{
    public class HomeController : Controller
    {
        private DoAnCNPMEntities3 db = new DoAnCNPMEntities3();

        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "LoginUser");
            }

            int totalFormIds = db.Forms.Count();

            int toatalDF = db.DetailForms.Count();
            int totalIsExamined = db.DetailForms.Count(f => f.IsExamined == true);
            int totalIsPaid = db.DetailForms.Count(f => f.IsPaid == true);

            int totalCustomerIds = db.Customers.Count();
            int totalStaffIds = db.Staffs.Count();
            int totalDoctorIds = db.Doctors.Count();

            var totalRevenue = db.DetailForms.Where(df => df.IsPaid == true).Sum(df => df.TotalMoney);

            ViewBag.TotalFormIds = totalFormIds;
            ViewBag.TotalDF = toatalDF;
            ViewBag.ExaminedCount = totalIsExamined;
            ViewBag.PaidCount = totalIsPaid;
            ViewBag.TotalCustomerIds = totalCustomerIds;
            ViewBag.TotalStaffIds = totalStaffIds;
            ViewBag.TotalDoctorIds = totalDoctorIds;
            ViewBag.TotalRevenue = totalRevenue;

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
    }
}