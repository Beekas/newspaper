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
using Newspaper.Models.ViewModel;

namespace Newspaper.Controllers
{
    [ValidateInput(false)]
    [SessionCheck(Role = "SuperAdmin,Supervisor,Admin")]
    public class SalesManController : Controller
    {
        private NewspaperEntities db = new NewspaperEntities();

        // GET: /SalesMan/
        public ActionResult Index()
        {
           if(Session["Category"].ToString() == "SuperAdmin")
            {
                var sales = db.SalesMan.ToList();
                return View(sales.ToList());

            }
          
            var salesman = db.SalesMan.ToList();


            
            return View(salesman.ToList());
             
        }

        // GET: /SalesMan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesMan salesman = db.SalesMan.Find(id);
            var cus = (from s in db.ServiceAssign
                      from c in db.Customer
                      from p in db.Service
                      
                       where  s.SalesManId == id && s.CustomerId == c.Id && s.NewspaperId==p.Id
                       select new
                       {
                           customer = c,
                           ServiceAssign = s,
                           service = p,
                          
                       }).ToList(); // as IEnumerable<CounterVM>;

            //var uniqCustomer = from m in cus
            //                   group m by new { m.customer.Id }
            //                      into mygroup
            //                   select mygroup.FirstOrDefault();

            ViewBag.Newspaper = new SelectList(db.Service.ToList(), "NewsPaperName", "NewspaperName");
            if (!(cus.Count == 0))
            {

                List<SalesmanVM> objConter = new List<SalesmanVM>();


                foreach (var item in cus)
                {
                    SalesmanVM counter = new SalesmanVM();
                    counter.salesman = salesman;
                    counter.Customertype = item.customer.CustomerType;
                    counter.CustomerId = item.customer.CustomerId;
                    counter.CustomerName = item.customer.FirstName;
                    counter.Address = item.customer.Address;
                    counter.Phone = item.customer.MPhone;
                    counter.Newspaper = item.service.NewsPaperName;
                    counter.Quantity = Convert.ToInt32(item.ServiceAssign.Quantity).ToString();
                    counter.Ended = item.ServiceAssign.EndedDate;
                    counter.EndedDate = ADTOBS.EngToNep(item.ServiceAssign.EndedDate).ToString();
                    counter.Paperdispatchdate = ADTOBS.EngToNep(item.ServiceAssign.PaperDispatchDate).ToString();

                    objConter.Add(counter);

                }
                ViewBag.errmsg = "customer";

                if (objConter.AsEnumerable() == null)
                {
                    return HttpNotFound();
                }
                return View(objConter.AsEnumerable());
            }
            else {
                SalesmanVM objsale = new SalesmanVM();
                List<SalesmanVM> objConter = new List<SalesmanVM>();
                objsale.salesman = salesman;

                objConter.Add(objsale);
                ViewBag.errmsg = "no cus";
               return View(objConter.AsEnumerable());
            }



            //if (salesman == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(salesman);
        }

        public ActionResult printallsalesman()
        {
            var saleman = db.SalesMan.ToList();
            return View(saleman.ToList());
        }

        // GET: /SalesMan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /SalesMan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,SalesManId,FullName,Email,BranchId,AltEmail,Phone1,Phone2")] SalesMan salesman)
        {
            if (ModelState.IsValid)
            {
                //salesman.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
               

                try
                {
                    db.SalesMan.Add(salesman);
                    db.SaveChanges();
                    //int BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                    String Operation = "Sales Man Created Sucessfully";
                    db.ActivityLog.Add(new ActivityLog
                    {
                        //BranchId = BranchId,
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

            return View(salesman);
        }

        // GET: /SalesMan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesMan salesman = db.SalesMan.Find(id);
            if (salesman == null)
            {
                return HttpNotFound();
            }
            return View(salesman);
        }

        // POST: /SalesMan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,SalesManId,FullName,BranchId,Email,AltEmail,Phone1,Phone2")] SalesMan salesman)
        {
            if (ModelState.IsValid)
            {
                //salesman.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                db.Entry(salesman).State = EntityState.Modified;
                try
                {
                
                    db.SaveChanges();
                    //int BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                    String Operation = "Sales Man Updated Sucessfully";
                    db.ActivityLog.Add(new ActivityLog
                    {
                        //BranchId = BranchId,
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
            return View(salesman);
        }

        // GET: /SalesMan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesMan salesman = db.SalesMan.Find(id);
            try
            {
                db.SalesMan.Remove(salesman);
                db.SaveChanges();
                String Operation = "Sales Man Deleted Sucessfully";
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

        // POST: /SalesMan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SalesMan salesman = db.SalesMan.Find(id);
            db.SalesMan.Remove(salesman);
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
