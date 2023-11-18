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
    public class DiseaseStatisticsController : Controller
    {
        private DoAnCNPMEntities3 db = new DoAnCNPMEntities3();

        public ActionResult Index(string SearchString)
        {
            var dSL = db.DiseaseStatistics.Include(d => d.DetailForm);
            if (!String.IsNullOrEmpty(SearchString))
            {
                dSL = dSL.Where(d => d.DSLId.ToString().Contains(SearchString)
                || d.Diagnose.Contains(SearchString)
                || d.DFId.ToString().Contains(SearchString)
                || d.DetailForm.Form.Patient.FullName.Contains(SearchString)
                || d.DetailForm.Form.Doctor.FullName.Contains(SearchString));
            }
            return View(dSL.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiseaseStatistic DiseaseStatistic = db.DiseaseStatistics.Find(id);
            if (DiseaseStatistic == null)
            {
                return HttpNotFound();
            }
            return View(DiseaseStatistic);
        }

        public ActionResult Create()
        {
            ViewBag.DFId = new SelectList(db.DetailForms, "DFId", "DFId");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DSLId,Diagnose,DFId")] DiseaseStatistic DiseaseStatistic)
        {
            if (ModelState.IsValid)
            {
                db.DiseaseStatistics.Add(DiseaseStatistic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DFId = new SelectList(db.DetailForms, "DFIdm", "DFId", DiseaseStatistic.DFId);
            return View(DiseaseStatistic);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiseaseStatistic DiseaseStatistic = db.DiseaseStatistics.Find(id);
            if (DiseaseStatistic == null)
            {
                return HttpNotFound();
            }
            ViewBag.DFId = new SelectList(db.DetailForms, "DFId", "DFId", DiseaseStatistic.DFId);
            return View(DiseaseStatistic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DSLId,Diagnose,DFId")] DiseaseStatistic DiseaseStatistic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(DiseaseStatistic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DFId = new SelectList(db.DetailForms, "DFId", "DFId", DiseaseStatistic.DFId);
            return View(DiseaseStatistic);
        }


        public ActionResult Delete(int id)
        {
            try
            {
                DiseaseStatistic dSL = db.DiseaseStatistics.Find(id);
                db.DiseaseStatistics.Remove(dSL);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index","Error");
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
