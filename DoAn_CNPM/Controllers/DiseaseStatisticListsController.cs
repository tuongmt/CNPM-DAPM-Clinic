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
    public class DiseaseStatisticListsController : Controller
    {
        private DoAnCNPMEntities3 db = new DoAnCNPMEntities3();

        // GET: DiseaseStatisticLists
        public ActionResult Index(string SearchString)
        {
            var dSL = db.DiseaseStatisticLists.Include(d => d.Form);
            if (!String.IsNullOrEmpty(SearchString))
            {
                dSL = dSL.Where(d => d.Dianose.Contains(SearchString)
                || d.Form.FormId.ToString().Contains(SearchString)
                || d.Form.Patient.FullName.Contains(SearchString)
                || d.Form.Doctor.FullName.Contains(SearchString));
            }
            return View(dSL.ToList());
        }

        // GET: DiseaseStatisticLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiseaseStatisticList diseaseStatisticList = db.DiseaseStatisticLists.Find(id);
            if (diseaseStatisticList == null)
            {
                return HttpNotFound();
            }
            return View(diseaseStatisticList);
        }

        // GET: DiseaseStatisticLists/Create
        public ActionResult Create()
        {
            ViewBag.FormId = new SelectList(db.DetailForms, "FormId", "FormId");
            return View();
        }

        // POST: DiseaseStatisticLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DSLId,Dianose,FormId")] DiseaseStatisticList diseaseStatisticList)
        {
            if (ModelState.IsValid)
            {
                db.DiseaseStatisticLists.Add(diseaseStatisticList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FormId = new SelectList(db.DetailForms, "FormIdm", "FormId", diseaseStatisticList.FormId);
            return View(diseaseStatisticList);
        }

        // GET: DiseaseStatisticLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiseaseStatisticList diseaseStatisticList = db.DiseaseStatisticLists.Find(id);
            if (diseaseStatisticList == null)
            {
                return HttpNotFound();
            }
            ViewBag.FormId = new SelectList(db.DetailForms, "FormId", "FormId", diseaseStatisticList.FormId);
            return View(diseaseStatisticList);
        }

        // POST: DiseaseStatisticLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DSLId,Dianose,FormId")] DiseaseStatisticList diseaseStatisticList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(diseaseStatisticList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FormId = new SelectList(db.DetailForms, "FormId", "FormId", diseaseStatisticList.FormId);
            return View(diseaseStatisticList);
        }

        // GET: DiseaseStatisticLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiseaseStatisticList diseaseStatisticList = db.DiseaseStatisticLists.Find(id);
            if (diseaseStatisticList == null)
            {
                return HttpNotFound();
            }
            return View(diseaseStatisticList);
        }

        // POST: DiseaseStatisticLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DiseaseStatisticList diseaseStatisticList = db.DiseaseStatisticLists.Find(id);
            db.DiseaseStatisticLists.Remove(diseaseStatisticList);
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
