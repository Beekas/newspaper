using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newspaper.Models;
using Newspaper.Models.DAL;

namespace Newspaper.Controllers
{
    public class NewspapersController : Controller
    {
        private NewspaperEntities db = new NewspaperEntities();

        // GET: Newspapers
        public ActionResult Index()
        {
            return View(db.Newspaper.ToList());
        }

        // GET: Newspapers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
             Newspaper.Models.Newspaper newspaper = db.Newspaper.Find(id);
            if (newspaper == null)
            {
                return HttpNotFound();
            }
            return View(newspaper);
        }

        // GET: Newspapers/Create
        public ActionResult Create()
        {
            Newspaper.Models.Newspaper newspaper = new Models.Newspaper();
            newspaper.CreatedDate = DateTime.Now;
            newspaper.CreatedBy = "aa";
            newspaper.UpdatedDate = DateTime.Now;
            newspaper.UpdatedBy = "aa";
            return View(newspaper);
        }

        // POST: Newspapers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NewspaperName,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate")] Newspaper.Models.Newspaper newspaper)
        {
            if (ModelState.IsValid)
            {
                
                db.Newspaper.Add(newspaper);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newspaper);
        }

        // GET: Newspapers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Newspaper.Models.Newspaper newspaper = db.Newspaper.Find(id);
            if (newspaper == null)
            {
                return HttpNotFound();
            }
            return View(newspaper);
        }

        // POST: Newspapers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NewspaperName,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate")] Newspaper.Models.Newspaper newspaper)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newspaper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newspaper);
        }

        // GET: Newspapers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Newspaper.Models.Newspaper newspaper = db.Newspaper.Find(id);
            if (newspaper == null)
            {
                return HttpNotFound();
            }
            return View(newspaper);
        }

        // POST: Newspapers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Newspaper.Models.Newspaper newspaper = db.Newspaper.Find(id);
            db.Newspaper.Remove(newspaper);
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
