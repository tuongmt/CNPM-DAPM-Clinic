using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace DoAn_CNPM.Models
{
    public class DeptsController : Controller
    {
        private DoAnCNPMEntities3 db = new DoAnCNPMEntities3();

        public ActionResult Index(string SearchString)
        {
            if (!String.IsNullOrEmpty(SearchString))
            {
                var cats = db.Depts.Where(s => s.DeptId.ToString().Contains(SearchString)
                || s.DeptName.ToString().Contains(SearchString));
                return View(cats.ToList());
            }
            return View(db.Depts.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DeptId,DeptName")] Dept Dept)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Depts.Add(Dept);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
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
            Dept Dept = db.Depts.Find(id);
            if (Dept == null)
            {
                return HttpNotFound();
            }
            return View(Dept);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DeptId,DeptName")] Dept Dept)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(Dept).State = EntityState.Modified;
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
                Dept Dept = db.Depts.Find(id);
                db.Depts.Remove(Dept);
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
