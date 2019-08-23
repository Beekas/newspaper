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
    [SessionCheck(Role = "SuperAdmin,Supervisor")]
    [ValidateInput(false)]
    public class PrakashanGroupsController : Controller
    {
        private activities log = new activities();

        private NewspaperEntities db = new NewspaperEntities();

        // GET: PrakashanGroups
        public ActionResult Index()
        {
            return View(db.prakashanGroups.ToList());
        }

        // GET: PrakashanGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrakashanGroup prakashanGroup = db.prakashanGroups.Find(id);
            if (prakashanGroup == null)
            {
                return HttpNotFound();
            }
            return View(prakashanGroup);
        }

        // GET: PrakashanGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PrakashanGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GroupId,GroupName")] PrakashanGroup prakashanGroup)
        {
            if (ModelState.IsValid)
            {
                db.prakashanGroups.Add(prakashanGroup);
                db.SaveChanges();
                TempData["message"] = "Prakahan Group created sucessfully";
                log.AddActivity("Group Name Created Successfuly");
                return RedirectToAction("Index");
            }

            return View(prakashanGroup);
        }

        // GET: PrakashanGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrakashanGroup prakashanGroup = db.prakashanGroups.Find(id);
            if (prakashanGroup == null)
            {
                return HttpNotFound();
            }
            return View(prakashanGroup);
        }

        // POST: PrakashanGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GroupId,GroupName")] PrakashanGroup prakashanGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prakashanGroup).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "Prakahan Group Edited successfully";
                log.AddActivity("Group Name Edited Successfully");
                return RedirectToAction("Index");
            }
            return View(prakashanGroup);
        }

        // GET: PrakashanGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrakashanGroup prakashanGroup = db.prakashanGroups.Find(id);
            if (prakashanGroup == null)
            {
                return HttpNotFound();
            }
            return View(prakashanGroup);
        }

        // POST: PrakashanGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PrakashanGroup prakashanGroup = db.prakashanGroups.Find(id);
            db.prakashanGroups.Remove(prakashanGroup);
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
