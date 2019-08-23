using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newspaper.Filters;
using Newspaper.Models;
using Newspaper.Models.DAL;

namespace Newspaper.Controllers
{
    [SessionCheck(Role = "SuperAdmin")]

    [ValidateInput(false)]
    public class FiscalYearsController : Controller
    {
        private activities log = new activities();
        private NewspaperEntities db = new NewspaperEntities();

        // GET: FiscalYears
        public ActionResult Index()
        {
            return View(db.FiscalYear.ToList());
        }

        [HttpPost]
        public JsonResult changeStatus(string fyId)
        {
            try
            {
                if (db.FiscalYear.Any(m => m.Status == true))
                {
                    var activeFiscalYear = db.FiscalYear.FirstOrDefault(m => m.Status == true);
                    activeFiscalYear.Status = false;
                }
                int id = Convert.ToInt32(fyId);
                var objFisYear = db.FiscalYear.Find(id);
                objFisYear.Status = true;
                db.SaveChanges();
                log.AddActivity("Fiscal Year changed sucessfully");
                return Json(true);

            }
            catch
            {
                return Json(false);
            }

        }



        // GET: FiscalYears/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FiscalYear fiscalYear = db.FiscalYear.Find(id);
            if (fiscalYear == null)
            {
                return HttpNotFound();
            }
            return View(fiscalYear);
        }

        // GET: FiscalYears/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FiscalYears/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NepYear,EngYear,Status,CreatedBy,EditedBy,CreatedDate,EditedDate")] FiscalYear fiscalYear)
        {
            if (ModelState.IsValid)
            {
                fiscalYear.CreatedBy = Session["userEmail"].ToString();
                fiscalYear.EditedBy = Session["userEmail"].ToString();
                fiscalYear.CreatedDate = DateTime.Now;
                fiscalYear.EditedDate = DateTime.Now;
                fiscalYear.Status = false;

                db.FiscalYear.Add(fiscalYear);
                db.SaveChanges();
                log.AddActivity("Fiscal Year created Successfully");
                return RedirectToAction("Index");
            }

            return View(fiscalYear);
        }

        // GET: FiscalYears/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FiscalYear fiscalYear = db.FiscalYear.Find(id);
            if (fiscalYear == null)
            {
                return HttpNotFound();
            }
            return View(fiscalYear);
        }

        // POST: FiscalYears/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NepYear,EngYear,Status,CreatedBy,EditedBy,CreatedDate,EditedDate")] FiscalYear fiscalYear)
        {

            if (ModelState.IsValid)
            {
                fiscalYear.CreatedDate = DateTime.Now;
                fiscalYear.EditedBy = Session["userEmail"].ToString();
                fiscalYear.EditedDate = DateTime.Now;
                db.Entry(fiscalYear).State = EntityState.Modified;
                db.SaveChanges();
                log.AddActivity("fiscal year edited successfully");
                return RedirectToAction("Index");
            }
            return View(fiscalYear);
        }

        // GET: FiscalYears/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FiscalYear fiscalYear = db.FiscalYear.Find(id);
            if (fiscalYear == null)
            {
                return HttpNotFound();
            }
            return View(fiscalYear);
        }

        // POST: FiscalYears/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FiscalYear fiscalYear = db.FiscalYear.Find(id);
            db.FiscalYear.Remove(fiscalYear);
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
