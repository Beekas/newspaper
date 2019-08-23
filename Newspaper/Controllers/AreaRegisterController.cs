using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newspaper.Models.ViewModel;
using Newspaper.Models.DAL;
using Newspaper.Filters;
using Newspaper.Models;

namespace Newspaper.Controllers
{
    [SessionCheck]
    [ValidateInput(false)]
    public class AreaRegisterController : Controller
    {

        private NewspaperEntities db = new NewspaperEntities();

        // GET: /AreaRegister/
        public ActionResult Index()
        {
            var arearegister = db.AreaRegister.Include(a => a.SaleMan);
            return View(arearegister.ToList());
        }

        // GET: /AreaRegister/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AreaRegister arearegister = db.AreaRegister.Find(id);
            if (arearegister == null)
            {
                return HttpNotFound();
            }
            return View(arearegister);
        }

        // GET: /AreaRegister/Create
        public ActionResult Create()
        {
            ViewBag.SalesManId = new SelectList(db.SalesMan, "Id", "FullName");
            return View();
        }

        // POST: /AreaRegister/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]

       
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SalesManId,Area,Comment,CreatedBy,EditedBy,ModifiedBy")] AreaRegister arearegister)
        {
            if (ModelState.IsValid)
            {
                arearegister.CreatedBy = Session["userEmail"].ToString();
                arearegister.EditedBy = Session["userEmail"].ToString();
                arearegister.ModifiedBy = Session["userEmail"].ToString();
                
                try
                {
                    db.AreaRegister.Add(arearegister);
                    db.SaveChanges();
                    String Operation = "Area Registered Sucessfully";
                    db.ActivityLog.Add(new ActivityLog
                    {
                        Operation = Operation,
                        CreatedBy = Session["userEmail"].ToString(),
                        CreatedDate = DateTime.Now

                    });
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }

            ViewBag.SalesManId = new SelectList(db.SalesMan, "Id", "FullName", arearegister.SalesManId);
            return View(arearegister);
        }

        // GET: /AreaRegister/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AreaRegister arearegister = db.AreaRegister.Find(id);
            if (arearegister == null)
            {
                return HttpNotFound();
            }
            ViewBag.SalesManId = new SelectList(db.SalesMan, "Id", "FullName", arearegister.SalesManId);
            return View(arearegister);
        }

        // POST: /AreaRegister/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SalesManId,Area,Comment,CreatedBy,EditedBy,ModifiedBy")] AreaRegister arearegister)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arearegister).State = EntityState.Modified;
                try
                {
                   
                    db.SaveChanges();
                    String Operation = "Area register sucessfully";
                    db.ActivityLog.Add(new ActivityLog
                    {
                        Operation = Operation,
                        CreatedBy = Session["userEmail"].ToString(),
                        CreatedDate = DateTime.Now

                    });
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            ViewBag.SalesManId = new SelectList(db.SalesMan, "Id", "FullName", arearegister.SalesManId);
            return View(arearegister);
        }

        // GET: /AreaRegister/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AreaRegister arearegister = db.AreaRegister.Find(id);
            try
            {
                db.AreaRegister.Remove(arearegister);
                db.SaveChanges();
                String Operation = "Area Deleted Sucessfully";
                db.ActivityLog.Add(new ActivityLog
                {
                    Operation = Operation,
                    CreatedBy = Session["userEmail"].ToString(),
                    CreatedDate = DateTime.Now

                });
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: /AreaRegister/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AreaRegister arearegister = db.AreaRegister.Find(id);
            
            try
            {
            
                db.SaveChanges();
                String Operation = "Area Deleted Sucessfully";
                db.ActivityLog.Add(new ActivityLog
                {
                    Operation = Operation,
                    CreatedBy = Session["userEmail"].ToString(),
                    CreatedDate = DateTime.Now

                });
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
