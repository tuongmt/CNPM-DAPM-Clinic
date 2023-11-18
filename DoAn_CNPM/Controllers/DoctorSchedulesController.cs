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
    public class DoctorSchedulesController : Controller
    {
        private DoAnCNPMEntities3 db = new DoAnCNPMEntities3();

        public ActionResult Index(string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                var dSchedules = db.DoctorSchedules.Where(ds => ds.DSId.ToString().Contains(searchString) ||
                ds.DoctorId.ToString().Contains(searchString) ||
                ds.DayOfWeek.Contains(searchString) ||
                ds.TimeStart.ToString().Contains(searchString) ||
                ds.TimeEnd.ToString().Contains(searchString));
                return View(dSchedules.ToList());
            }
            var doctorSchedules = db.DoctorSchedules.Include(d => d.Doctor);
            return View(doctorSchedules.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorSchedule doctorSchedule = db.DoctorSchedules.Find(id);
            if (doctorSchedule == null)
            {
                return HttpNotFound();
            }
            return View(doctorSchedule);
        }

        public ActionResult Create()
        {
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "FullName");
            ViewBag.DayOfWeek = new SelectList(new List<SelectListItem>
            {
               new SelectListItem { Text = "Sáng thứ hai", Value = "Sáng thứ hai" },
               new SelectListItem { Text = "Chiều thứ hai", Value = "Chiều thứ hai" },
               new SelectListItem { Text = "Sáng thứ ba", Value = "Sáng thứ ba" },
               new SelectListItem { Text = "Chiều thứ ba", Value = "Chiều thứ ba" },
               new SelectListItem { Text = "Sáng thứ tư", Value = "Sáng thứ tư" },
               new SelectListItem { Text = "Chiều thứ tư", Value = "Chiều thứ tư" },
               new SelectListItem { Text = "Sáng thứ năm", Value = "Sáng thứ năm" },
               new SelectListItem { Text = "Chiều thứ năm", Value = "Chiều thứ năm" },
               new SelectListItem { Text = "Sáng thứ sáu", Value = "Sáng thứ sáu" },
               new SelectListItem { Text = "Chiều thứ sáu", Value = "Chiều thứ sáu" },
               new SelectListItem { Text = "Sáng thứ bảy", Value = "Sáng thứ bảy" },
               new SelectListItem { Text = "Chiều thứ bảy", Value = "Chiều thứ bảy" },
            }, "Value", "Text");
            ViewBag.TimeStart = new SelectList(new List<SelectListItem>
             {
                  new SelectListItem { Text = "8:00", Value = "8:00" },
            }, "Value", "Text");
            ViewBag.TimeEnd = new SelectList(new List<SelectListItem>
             {
                  new SelectListItem { Text = "17:00", Value = "17:00" },
            }, "Value", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DSId,DoctorId,DayOfWeek,TimeStart,TimeEnd")] DoctorSchedule doctorSchedule)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.DoctorSchedules.Add(doctorSchedule);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "FullName", doctorSchedule.DoctorId);

                return View(doctorSchedule);
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }           
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorSchedule doctorSchedule = db.DoctorSchedules.Find(id);
            if (doctorSchedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "FullName", doctorSchedule.DoctorId);
            ViewBag.DayOfWeek = new SelectList(new List<SelectListItem>
            {
               new SelectListItem { Text = "Sáng thứ hai", Value = "Sáng thứ hai" },
               new SelectListItem { Text = "Chiều thứ hai", Value = "Chiều thứ hai" },
               new SelectListItem { Text = "Sáng thứ ba", Value = "Sáng thứ ba" },
               new SelectListItem { Text = "Chiều thứ ba", Value = "Chiều thứ ba" },
               new SelectListItem { Text = "Sáng thứ tư", Value = "Sáng thứ tư" },
               new SelectListItem { Text = "Chiều thứ tư", Value = "Chiều thứ tư" },
               new SelectListItem { Text = "Sáng thứ năm", Value = "Sáng thứ năm" },
               new SelectListItem { Text = "Chiều thứ năm", Value = "Chiều thứ năm" },
               new SelectListItem { Text = "Sáng thứ sáu", Value = "Sáng thứ sáu" },
               new SelectListItem { Text = "Chiều thứ sáu", Value = "Chiều thứ sáu" },
               new SelectListItem { Text = "Sáng thứ bảy", Value = "Sáng thứ bảy" },
               new SelectListItem { Text = "Chiều thứ bảy", Value = "Chiều thứ bảy" },
            }, "Value", "Text");
            ViewBag.TimeStart = new SelectList(new List<SelectListItem>
             {
                  new SelectListItem { Text = "8:00", Value = "8:00" },
            }, "Value", "Text");
            ViewBag.TimeEnd = new SelectList(new List<SelectListItem>
             {
                  new SelectListItem { Text = "17:00", Value = "17:00" },
            }, "Value", "Text");
            return View(doctorSchedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DSId,DoctorId,DayOfWeek,TimeStart,TimeEnd")] DoctorSchedule doctorSchedule)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(doctorSchedule).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "FullName", doctorSchedule.DoctorId);
                return View(doctorSchedule);
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
                DoctorSchedule doctorSchedule = db.DoctorSchedules.Find(id);
                db.DoctorSchedules.Remove(doctorSchedule);
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
