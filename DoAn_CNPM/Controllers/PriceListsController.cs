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

        public ActionResult Index(string SearchString)
        {
            var priceLists = db.PriceLists.Include(p => p.Dept);
            if (!String.IsNullOrEmpty(SearchString))
            {
                priceLists = priceLists.Where(p => p.PriceListId.ToString().Contains(SearchString)
                || p.PriceListName.Contains(SearchString)
                || p.Dept.DeptName.Contains(SearchString)
                || p.Price.ToString().Contains(SearchString));
            }

            return View(priceLists.ToList());
        }

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

        public ActionResult Create()
        {
            ViewBag.DeptId = new SelectList(db.Depts, "DeptId", "DeptName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PriceListId,PriceListName,Price,DeptId")] PriceList priceList)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.PriceLists.Add(priceList);
                    db.SaveChanges();                   
                }
                ViewBag.DeptId = new SelectList(db.Depts, "DeptId", "DeptName", priceList.DeptId);
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
            PriceList priceList = db.PriceLists.Find(id);
            if (priceList == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeptId = new SelectList(db.Depts, "DeptId", "DeptName", priceList.DeptId);
            return View(priceList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PriceListId,PriceListName,Price,DeptId")] PriceList priceList)
        {
            try {
                if (ModelState.IsValid)
                {
                    db.Entry(priceList).State = EntityState.Modified;
                    db.SaveChanges();                 
                }
                ViewBag.DeptId = new SelectList(db.Depts, "DeptId", "DeptName", priceList.DeptId);
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
                PriceList priceList = db.PriceLists.Find(id);
                db.PriceLists.Remove(priceList);
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
