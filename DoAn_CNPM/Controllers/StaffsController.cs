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
    public class StaffsController : Controller
    {
        private DoAnCNPMEntities3 db = new DoAnCNPMEntities3();

        public ActionResult Index(string SearchString)
        {
            if (!String.IsNullOrEmpty(SearchString))
            {
                var staff = db.Staffs.Where(s => s.StaffId.ToString().Contains(SearchString)
                || s.FullName.Contains(SearchString)
                || s.Gender.Contains(SearchString)
                || s.Phone.Contains(SearchString)
                || s.Position.Contains(SearchString));
                return View(staff.ToList());

            }
            return View(db.Staffs.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        public ActionResult Create()
        {
            ViewBag.Gender = new SelectList(new List<SelectListItem>
            {
               new SelectListItem { Text = "Nam", Value = "Nam" },
               new SelectListItem { Text = "Nữ", Value = "Nữ" }
            }, "Value", "Text");

            ViewBag.Position = new SelectList(new List<SelectListItem>
            {
               new SelectListItem { Text = "Lễ tân - Receptionist", Value = "Lễ tân - Receptionist" },
               new SelectListItem { Text = "Điều dưỡng - Nursing", Value = "Điều dưỡng - Nursing" },
               new SelectListItem { Text = "Trợ lý y tế - Medical Assistant", Value = "Trợ lý y tế - Medical Assistant" }
            }, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StaffId,FullName,Gender,Phone,Position")] Staff staff)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Staffs.Add(staff);
                    db.SaveChanges();
                }
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

            Staff staff = db.Staffs.Find(id);

            if (staff == null)
            {
                return HttpNotFound();
            }

            if (staff.Gender.ToString() == "Nam")
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

            if(staff.Position.ToString().Contains("Lễ tân"))
            {
                ViewBag.Position = new SelectList(new List<SelectListItem>
                {
                 new SelectListItem { Text = "Lễ tân", Value = "Lễ tân - Receptionist" },
                 new SelectListItem { Text = "Điều dưỡng", Value = "Điều dưỡng - Nursing" },
                 new SelectListItem { Text = "Trợ lý y tế", Value = "Trợ lý y tế - Medical Assistant" }
                }, "Value", "Text");
            }
            else if(staff.Position.ToString().Contains("Điều dưỡng"))
            {
                ViewBag.Position = new SelectList(new List<SelectListItem>
                {
                 new SelectListItem { Text = "Điều dưỡng", Value = "Điều dưỡng - Nursing" },
                 new SelectListItem { Text = "Lễ tân", Value = "Lễ tân - Receptionist" },
                 new SelectListItem { Text = "Trợ lý y tế", Value = "Trợ lý y tế - Medical Assistant" }
                }, "Value", "Text");
            }
            else
            {
                ViewBag.Position = new SelectList(new List<SelectListItem>
                {
                 new SelectListItem { Text = "Trợ lý y tế", Value = "Trợ lý y tế - Medical Assistant" },
                 new SelectListItem { Text = "Lễ tân", Value = "Lễ tân - Receptionist" },
                 new SelectListItem { Text = "Điều dưỡng", Value = "Điều dưỡng - Nursing" }
                }, "Value", "Text");
            }

            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StaffId,FullName,Gender,Phone,Position")] Staff staff)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(staff).State = EntityState.Modified;
                    db.SaveChanges();                    
                }
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
                Staff staff = db.Staffs.Find(id);
                db.Staffs.Remove(staff);
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
