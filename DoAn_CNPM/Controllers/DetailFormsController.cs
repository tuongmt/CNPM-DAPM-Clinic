using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAn_CNPM.Models;
using PagedList;

namespace DoAn_CNPM.Controllers
{
    public class DetailFormsController : Controller
    {
        private DoAnCNPMEntities3 db = new DoAnCNPMEntities3();

        [HttpPost]
        public ActionResult UpdateExaminedStatus(int id)
        {
            var f = db.DetailForms.Find(id);
            if (f == null)
            {
                return HttpNotFound();
            }

            f.IsExamined = !f.IsExamined;
            db.Entry(f).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdatePaymentStatus(int id)
        {
            var f = db.DetailForms.Find(id);
            if (f == null)
            {
                return HttpNotFound();
            }

            f.IsPaid = !f.IsPaid;
            db.Entry(f).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: DetailForms
        public ActionResult Index(string searchString, int? page, double min = double.MinValue, double max = double.MaxValue, int checkSort = 0)
        {
            
            var detailForms = db.DetailForms.Include(d => d.Form).Include(d => d.PriceList);
            //sorting
            detailForms = detailForms.OrderBy(d => d.DFId);
            if (!String.IsNullOrEmpty(searchString))
            {
                detailForms = detailForms.Where(d => d.FormId.ToString().Contains(searchString)
                || d.TotalMoney.ToString().Contains(searchString));
            }
            //if (min >= 0 && max > 0)
            //{
            //    detailForms = db.DetailForms.OrderByDescending(d => d.TotalMoney).Where(p => (double)p.TotalMoney >= min && (double)p.TotalMoney <= max);
            //}

            int pageSize = 6;

            int pageNumber = (page ?? 1);

            if (page == null) page = 1;

            //return View(detailForms.ToList());
            return View("Index",detailForms.ToPagedList(pageNumber, pageSize));
        }

        // GET: DetailForms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetailForm detailForm = db.DetailForms.Find(id);
            if (detailForm == null)
            {
                return HttpNotFound();
            }
            return View(detailForm);
        }

        // GET: DetailForms/Create
        public ActionResult Create()
        {
            ViewBag.FormId = new SelectList(db.Forms, "FormId", "FormId");
            ViewBag.PriceListId = new SelectList(db.PriceLists, "PriceListId", "PriceListName");

            
            return View();
        }

        // POST: DetailForms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DFId,Quantity,FormId,PriceListId,TotalMoney")] DetailForm detailForm)
        {
            if (ModelState.IsValid)
            {
               
                db.DetailForms.Add(detailForm);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.FormId = new SelectList(db.Forms, "FormId", "FormId", detailForm.FormId);
            ViewBag.PriceListId = new SelectList(db.PriceLists, "PriceListId", "PriceListName", detailForm.PriceListId);
            ViewBag.Price = detailForm.PriceList.Price.ToString();
            return View(detailForm);
        }

        // GET: DetailForms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetailForm detailForm = db.DetailForms.Find(id);
            if (detailForm == null)
            {
                return HttpNotFound();
            }
            ViewBag.FormId = new SelectList(db.Forms, "FormId", "FormId", detailForm.FormId);
            ViewBag.PriceListId = new SelectList(db.PriceLists, "PriceListId", "PriceListName", detailForm.PriceListId);
            return View(detailForm);
        }

        // POST: DetailForms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DFId,Quantity,FormId,PriceListId,TotalMoney")] DetailForm detailForm)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detailForm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FormId = new SelectList(db.Forms, "FormId", "FormId", detailForm.FormId);
            ViewBag.PriceListId = new SelectList(db.PriceLists, "PriceListId", "PriceListName", detailForm.PriceListId);
            return View(detailForm);
        }

        // GET: DetailForms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetailForm detailForm = db.DetailForms.Find(id);
            if (detailForm == null)
            {
                return HttpNotFound();
            }
            return View(detailForm);
        }

        // POST: DetailForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DetailForm detailForm = db.DetailForms.Find(id);
            db.DetailForms.Remove(detailForm);
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
