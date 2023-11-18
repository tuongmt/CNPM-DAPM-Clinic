using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DoAn_CNPM.Models;
using PagedList;

namespace DoAn_CNPM.Controllers
{
    public class DetailFormsController : Controller
    {
        private DoAnCNPMEntities3 db = new DoAnCNPMEntities3();
        private int pageSize = 6;
        private int pageNumber; 

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

        public ActionResult Index(string searchString, string searchBy, int? page)      
        {
            var detailForms = db.DetailForms.Include(d => d.Form).Include(d => d.PriceList);

            pageNumber = (page ?? 1);

            if (page == null) page = 1;

            //sorting
            detailForms = detailForms.OrderBy(d => d.DFId);

            if (searchBy == "All")
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    detailForms = detailForms.Where(d => d.DFId.ToString().Contains(searchString)
                    || d.FormId.ToString().Contains(searchString)
                    || d.FOId.ToString().Contains(searchString)
                    || d.TotalMoney.ToString().Trim().Contains(searchString)
                    || d.IsExamined.ToString().Contains(searchString)
                    || d.IsPaid.ToString().Contains(searchString));
                }
                return View("Index", detailForms.ToPagedList(pageNumber, pageSize));
            }
            else if(searchBy == "Id_CTĐK")
            {
                return View(detailForms.Where(d => d.DFId.ToString().Contains(searchString) || searchString == null).ToPagedList(pageNumber, pageSize));
            }else if(searchBy == "Id_ĐK")
            {
                return View(detailForms.Where(d => d.FormId.ToString().Contains(searchString) || searchString == null).ToPagedList(pageNumber, pageSize));
            }
            else if (searchBy == "Id_ĐKTT")
            {
                return View(detailForms.Where(d => d.FOId.ToString().Contains(searchString) || searchString == null).ToPagedList(pageNumber, pageSize));
            }else if (searchBy == "TotalMoney")
            {
                return View(detailForms.Where(d => d.TotalMoney.ToString().Contains(searchString) || searchString == null).ToPagedList(pageNumber, pageSize));
            }
            else if (searchBy == "ExamStatus")
            {
                return View(detailForms.Where(d => d.IsExamined.ToString().Contains(searchString) || searchString == null).ToPagedList(pageNumber, pageSize));
            }
            else if (searchBy == "PayStatus")
            {
                return View(detailForms.Where(d => d.IsPaid.ToString().Contains(searchString) || searchString == null).ToPagedList(pageNumber, pageSize));
            }
            else if (searchBy == "SL")
            {
                return View(detailForms.Where(d => d.Quantity.ToString().Contains(searchString) || searchString == null).ToPagedList(pageNumber, pageSize));
            }

            return View("Index",detailForms.ToPagedList(pageNumber, pageSize));
        }

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

        public double GetPrice(int priceListId)
        {
            var price = db.PriceLists.FirstOrDefault(p => p.PriceListId == priceListId)?.Price;
            return price ?? 0; // Trả về 0 nếu không tìm thấy giá
        }

        public ActionResult Create()
        {
            ViewBag.FormId = new SelectList(db.Forms, "FormId", "FormId");
            ViewBag.FOId = new SelectList(db.FormOnlines, "FOId", "FOId");
            ViewBag.PriceListId = new SelectList(db.PriceLists, "PriceListId", "PriceListName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DFId,Quantity,FormId,FOId,PriceListId,TotalMoney")] DetailForm detailForm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.FormId = new SelectList(db.Forms, "FormId", "FormId", detailForm.FormId);
                    ViewBag.FOId = new SelectList(db.FormOnlines, "FOId", "FOId", detailForm.FOId);
                    ViewBag.PriceListId = new SelectList(db.PriceLists, "PriceListId", "PriceListName", detailForm.PriceListId);
                    db.DetailForms.Add(detailForm);
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
            DetailForm detailForm = db.DetailForms.Find(id);
            if (detailForm == null)
            {
                return HttpNotFound();
            }
            ViewBag.FormId = new SelectList(db.Forms, "FormId", "FormId", detailForm.FormId);
            ViewBag.FOId = new SelectList(db.FormOnlines, "FOId", "FOId", detailForm.FOId);
            ViewBag.PriceListId = new SelectList(db.PriceLists, "PriceListId", "PriceListName", detailForm.PriceListId);
            return View(detailForm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DFId,Quantity,FormId,FOId,PriceListId,TotalMoney")] DetailForm detailForm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(detailForm).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                RedirectToAction("Index", "Error");
            }
            ViewBag.FormId = new SelectList(db.Forms, "FormId", "FormId", detailForm.FormId);
            ViewBag.FOId = new SelectList(db.FormOnlines, "FOId", "FOId", detailForm.FOId);
            ViewBag.PriceListId = new SelectList(db.PriceLists, "PriceListId", "PriceListName", detailForm.PriceListId);
            return View(detailForm);
        }

        public ActionResult Delete(int id)
        {
            try {
                DetailForm detailForm = db.DetailForms.Find(id);
                db.DetailForms.Remove(detailForm);
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
