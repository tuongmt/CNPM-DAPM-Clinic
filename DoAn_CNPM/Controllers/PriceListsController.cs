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
    public class PriceListsController : Controller
    {
        private DoAnCNPMEntities3 db = new DoAnCNPMEntities3();

        // GET: PriceLists
        public ActionResult Index(string SearchString)
        {
            var priceLists = db.PriceLists.Include(p => p.Category);
            if (!String.IsNullOrEmpty(SearchString))
            {
                priceLists = priceLists.Where(s => s.NamePriceList.Contains(SearchString));
            }

            return View(priceLists.ToList());
        }

        // GET: PriceLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriceList priceList = db.PriceLists.Find(id);
            if (priceList == null)
            {
                return HttpNotFound();
            }
            return View(priceList);
        }

        // GET: PriceLists/Create
        public ActionResult Create()
        {
            ViewBag.IdCat = new SelectList(db.Categories, "IdCat", "NameCat");
            return View();
        }

        // POST: PriceLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPriceList,NamePriceList,Price,IdCat")] PriceList priceList)
        {
            if (ModelState.IsValid)
            {
                db.PriceLists.Add(priceList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCat = new SelectList(db.Categories, "IdCat", "NameCat", priceList.IdCat);
            return View(priceList);
        }

        // GET: PriceLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriceList priceList = db.PriceLists.Find(id);
            if (priceList == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCat = new SelectList(db.Categories, "IdCat", "NameCat", priceList.IdCat);
            return View(priceList);
        }

        // POST: PriceLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPriceList,NamePriceList,Price,IdCat")] PriceList priceList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(priceList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCat = new SelectList(db.Categories, "IdCat", "NameCat", priceList.IdCat);
            return View(priceList);
        }

        // GET: PriceLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriceList priceList = db.PriceLists.Find(id);
            if (priceList == null)
            {
                return HttpNotFound();
            }
            return View(priceList);
        }

        // POST: PriceLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PriceList priceList = db.PriceLists.Find(id);
            db.PriceLists.Remove(priceList);
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
