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
    [SessionCheck(Role ="SuperAdmin,Supervisor")]
    [ValidateInput(false)]
    public class GroupNamesController : Controller
    {
        private NewspaperEntities db = new NewspaperEntities();
        private activities log = new activities();

        // GET: GroupNames
        public ActionResult Index()
        {
            var groupNames = db.groupNames.Include(g => g.Route);
            return View(groupNames.ToList());
        }

        // GET: GroupNames/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupName groupName = db.groupNames.Find(id);
            if (groupName == null)
            {
                return HttpNotFound();
            }
            return View(groupName);
        }

        // GET: GroupNames/Create
        public ActionResult Create()
        {
            ViewBag.RouteId = new SelectList(db.Routes, "Id", "RouteName");
            return View();
        }

        // POST: GroupNames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AgentName,Pan,GP_Quantity,RN_Quantity,MUNA_Quantity,Madhu_Quantity,Yuwa_Quantity,Address,Time,RouteName,Transport,State,District,Phone,Email,URL,RouteId")] GroupName groupName)
        {
            if (ModelState.IsValid)
            {
                groupName.Route = db.Routes.Find(groupName.RouteId);
                db.groupNames.Add(groupName);
                db.SaveChanges();
                TempData["message"] = "Agent created successfully";
                log.AddActivity("Agent Names Created Successfully");
                return RedirectToAction("Index");
            }

            ViewBag.RouteId = new SelectList(db.Routes, "Id", "RouteName", groupName.RouteId);
            return View(groupName);
        }

        // GET: GroupNames/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.RouteId = new SelectList(db.Routes, "Id", "RouteName");
            GroupName groupName = db.groupNames.Find(id);
            
            if (groupName == null)
            {
                return HttpNotFound();
            }
            ViewBag.RouteId = new SelectList(db.Routes, "Id", "RouteName", groupName.RouteId);
            return View(groupName);
        }

        // POST: GroupNames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AgentName,Pan,GP_Quantity,RN_Quantity,MUNA_Quantity,Madhu_Quantity,Yuwa_Quantity,Address,Time,RouteName,Transport,State,District,Phone,Email,URL,RouteId")] GroupName groupName)
        {
            if (ModelState.IsValid)
            {
                groupName.Route = db.Routes.Find(groupName.RouteId);
                db.Entry(groupName).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "Agent Edited successfully";
                log.AddActivity("Agent Name Edited Successfully");
                return RedirectToAction("Index");
            }
            ViewBag.RouteId = new SelectList(db.Routes, "Id", "RouteName", groupName.RouteName);
            return View(groupName);
        }

        // GET: GroupNames/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupName groupName = db.groupNames.Find(id);
            if (groupName == null)
            {
                return HttpNotFound();
            }
            return View(groupName);
        }

        // POST: GroupNames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GroupName groupName = db.groupNames.Find(id);
            db.groupNames.Remove(groupName);
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
