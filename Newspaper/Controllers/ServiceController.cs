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
using Newspaper.Filters;
using System.IO;

namespace Newspaper.Controllers
{
    [SessionCheck(Role = "SuperAdmin,Supervisor,Admin")]
    [ValidateInput(false)]
    public class ServiceController : Controller
    {
        private NewspaperEntities db = new NewspaperEntities();

        // GET: /Service/
        public ActionResult Index()
        {
            return View(db.Service.ToList());
        }

        // GET: /Service/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Service.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // GET: /Service/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Service/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,ServiceCode,NewsPaperName,TimeBase,CreatedBy,EditedBy,EditedDate,images")] Service service,HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
               
                service.CreatedBy = Session["userEmail"].ToString();
                service.EditedBy = Session["userEmail"].ToString();
                service.EditedDate = DateTime.Now;


                HttpFileCollectionBase image = Request.Files;
                string filename = image[0].FileName; /* service.NewsPaperName + ".jpg";*/
                string name = "/Images/Newspaper/";
                bool exist = System.IO.Directory.Exists(Server.MapPath(name));
                if (!exist)
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(name));
                }
                service.Images = "/Images/Newspaper/" + filename;
                if (ImageFile != null)
                {
                    filename = Path.Combine(Server.MapPath("~/Images/Newspaper/"), filename);
                   ImageFile.SaveAs(filename);
                }

                try
                {
                    db.Service.Add(service);
                    db.SaveChanges();
                    String Operation = "Service Created Sucessfully";
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

            return View(service);
        }

        // GET: /Service/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Service.Find(id);
            if (service.Images == null)
            {
                string filename1 = service.Images;
                service.Images = "/Images/Newspaper/" + filename1;
            }
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: /Service/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,ServiceCode,NewsPaperName,TimeBase,CreatedBy,EditedBy,EditedDate,Images")] Service service, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service).State = EntityState.Modified;
                HttpFileCollectionBase image = Request.Files;
                if (ImageFile != null)
                {

                    string fileName1 = image[0].FileName;
                    ///string filename = Path.GetFileNameWithoutExtension(admin.ImageFile.FileName);
                    //string extension = Path.GetExtension(admin.ImageFile.FileName);
                    //filename = admin.Id + extension;
                    service.Images = "/Images/Newspaper/" + fileName1;
                    string filename = Path.Combine(Server.MapPath("~/Images/Newspaper/"), fileName1);
                    ImageFile.SaveAs(filename);
                }

                try
                {
                  
                    db.SaveChanges();
                    String Operation = "Service Updated Sucessfully";
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
            return View(service);
        }

        // GET: /Service/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Service.Find(id);
            try
            {
                db.Service.Remove(service);
                db.SaveChanges();
                String Operation = "Service Deleted Sucessfully";
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

        // POST: /Service/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Service service = db.Service.Find(id);
            db.Service.Remove(service);
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
