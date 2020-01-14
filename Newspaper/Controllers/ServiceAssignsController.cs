using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newspaper.Models;
using Newspaper.Models.ViewModel;
using Newspaper.Models.DAL;
using AutoMapper;
using Newspaper.Filters;

namespace Newspaper.Controllers
{
    [ValidateInput(false)]
    [SessionCheck(Role = "SuperAdmin,Supervisor,Admin")]
    public class ServiceAssignsController : Controller
    {
        private activities log = new activities();

        private NewspaperEntities db = new NewspaperEntities();


        public ActionResult Show()
        {

            return View(db.Customer.ToList());
        }


        public ActionResult AllAssigned()

        {
            var cus = (from s in db.ServiceAssign
                       from c in db.Customer

                       from n in db.Service
                       from o in db.SalesMan
                       where s.NewspaperId == n.Id && s.CustomerId == c.Id && s.SalesManId == o.Id
                       select new
                       {
                           Customer = c,
                           ServiceAssign = s,
                           NewsPaper = n,
                           SalesMan = o
                       }).ToList();
            List<assignNewspaperVM> objConter = new List<assignNewspaperVM>();
            foreach (var item in cus)
            {
                assignNewspaperVM counter = new assignNewspaperVM();
                counter.Paper = item.NewsPaper.NewsPaperName;
                counter.phone = item.Customer.MPhone;
                counter.CustomerName = item.Customer.FirstName;
                counter.Newspapername = item.NewsPaper.NewsPaperName;
                counter.Address = item.Customer.Address;
                counter.Distributor = item.SalesMan.FullName;

                counter.EndedDate = item.ServiceAssign.EndedDate;
                counter.PaperDispatchDate = item.ServiceAssign.PaperDispatchDate;
                var date = item.ServiceAssign.EndedDate;
                counter.endDate = ADTOBS.EngToNep(date).ToString();

                var dispatch = item.ServiceAssign.PaperDispatchDate;
                counter.dispatch = ADTOBS.EngToNep(dispatch).ToString();

                counter.CusId = item.Customer.CustomerId;
                counter.CustomerId = item.Customer.Id;
                objConter.Add(counter);

            }
            return View(objConter.AsEnumerable());


        }


        // GET: ServiceAssigns
        public ActionResult Index()
        {
            //var cus =  (from s in db.ServiceAssign
            //                      from c in db.Customer

            //                      from n in db.Service
            //                      from o in db.SalesMan
            //                      where s.NewspaperId == n.Id && s.CustomerId == c.Id && s.SalesManId == o.Id
            //                      select new
            //                      {
            //                          Customer = c,
            //                          ServiceAssign = s,
            //                          NewsPaper = n,
            //                          SalesMan = o
            //                      }).ToList();


            //var uniqCustomer =(from m in cus
            //    group m by new { m.Customer.CustomerId }
            //                       into mygroup
            //               select mygroup.FirstOrDefault()).ToList(); 

            var cus = (from s in db.ServiceAssign
                       from c in db.Customer
                       from n in db.Service
                       from i in db.SalesMan
                       where s.NewspaperId == n.Id && s.CustomerId == c.Id && s.SalesManId == i.Id
                       select new
                       {
                           Customer = c,
                           ServiceAssign = s,
                           NewsPaper = n,
                           SalesMan = i
                       }).ToList();
            var uniqCustomer = from m in cus
                               group m by new { m.Customer.Id }
                               into mygroup
                               select mygroup.FirstOrDefault();

            List<assignNewspaperVM> objConter = new List<assignNewspaperVM>();


            foreach (var item in uniqCustomer)
            {
                assignNewspaperVM counter = new assignNewspaperVM();
                counter.Paper = item.NewsPaper.NewsPaperName;
                counter.phone = item.Customer.MPhone;
                counter.CustomerType = item.Customer.CustomerType;
                counter.CustomerName = item.Customer.FirstName;
                counter.Newspapername = item.NewsPaper.NewsPaperName;
                counter.Address = item.Customer.Address;
                counter.Distributor = item.SalesMan.FullName;
                counter.EndedDate = item.ServiceAssign.EndedDate;
                counter.PaperDispatchDate = item.ServiceAssign.PaperDispatchDate;



                //var dispatch = item.ServiceAssign.PaperDispatchDate;
                //counter.dispatch = ADTOBS.EngToNep(dispatch).ToString();

                counter.CusId = item.Customer.CustomerId;
                counter.CustomerId = item.Customer.Id;
                objConter.Add(counter);

            }
            return View(objConter.AsEnumerable());


            //return View(db.ServiceAssign.ToList());
        }


        public ActionResult CustomerDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var cus = (from s in db.ServiceAssign
                       from c in db.Customer
                       from n in db.Service
                       where s.NewspaperId == n.Id && s.CustomerId == c.Id && s.CustomerId == id
                       select new
                       {
                           Customer = c,
                           ServiceAssign = s,
                           NewsPaper = n
                       }).ToList(); // as IEnumerable<CounterVM>;



            List<CounterVM> objConter = new List<CounterVM>();


            foreach (var item in cus)
            {
                CounterVM counter = new CounterVM();
                counter.NewsPaper = item.NewsPaper;
                counter.Customer = item.Customer;
                counter.ServiceAssign = item.ServiceAssign; var dispatch = item.ServiceAssign.PaperDispatchDate;
                counter.Paperdispatch = ADTOBS.EngToNep(dispatch).ToString();
                var date = item.ServiceAssign.EndedDate;
                counter.enddate = ADTOBS.EngToNep(date).ToString();
                objConter.Add(counter);

            }


            if (objConter.AsEnumerable() == null)
            {
                return HttpNotFound();
            }
            return View(objConter.AsEnumerable());
        }



        // GET: ServiceAssigns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceAssign serviceAssign = db.ServiceAssign.Find(id);
            if (serviceAssign == null)
            {
                return HttpNotFound();
            }
            return View(serviceAssign);
        }

        public ActionResult ChangeSaleman(int? id)
        {
            var date = DateTime.Now;
            assignNewspaperVM assignVM = new assignNewspaperVM();
            assignVM.CustomerId = Convert.ToInt32(id);
            var cus = (from s in db.ServiceAssign
                       from c in db.Customer
                       from n in db.Service
                       where s.NewspaperId == n.Id && s.CustomerId == c.Id && s.CustomerId == id
                       select new
                       {
                           n

                       }).ToList();
            List<Service> ser = new List<Service>();
            foreach (var item in cus)
            {
                Service newSer = db.Service.Find(item.n.Id);
                ser.Add(newSer);
            }
            assignVM.custId = db.Customer.Find(id).CustomerId;
            assignVM.Newspapers = new SelectList(ser, "Id", "NewsPaperName");
            assignVM.Salesmans = new SelectList(db.SalesMan, "Id", "FullName");
            return View(assignVM);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeSaleman(assignNewspaperVM serviceAssign)
        {



            if (db.ServiceAssign.Any(m => m.CustomerId == serviceAssign.CustomerId))
            {
                foreach (var item in serviceAssign.NewspaperIds)
                {
                    ServiceAssign objService = db.ServiceAssign.FirstOrDefault(m => m.CustomerId == serviceAssign.CustomerId && m.NewspaperId == item);


                    objService.SalesManId = serviceAssign.SalesManId;


                    log.AddActivity("Service assign to customer successfully");
                    db.SaveChanges();
                }
                return RedirectToAction("Index");

            }
            else
            {
                ModelState.AddModelError("", "Newspaper Already Assigned.");
                serviceAssign.Newspapers = new SelectList(db.Service, "Id", "NewsPaperName");
                serviceAssign.Salesmans = new SelectList(db.SalesMan, "Id", "FullName");
                return View(serviceAssign);
            }



        }




        // GET: ServiceAssigns/Create
        public ActionResult Create(int id)
        {
            var date = DateTime.Now;
            assignNewspaperVM assignVM = new assignNewspaperVM();
            assignVM.CustomerId = id;
            assignVM.Newspapers = new SelectList(db.Service, "Id", "NewsPaperName");
            assignVM.Salesmans = new SelectList(db.SalesMan, "Id", "FullName");

            assignVM.CreatedDate = date.Date;
            assignVM.EndedDate = DateTime.Now;
            assignVM.CreatedBy = Session["userEmail"].ToString();
            assignVM.UpdatedBy = Session["userEmail"].ToString();
            assignVM.UpdatedDate = DateTime.Now;
            assignVM.UpdatedDate = DateTime.Now;
            assignVM.status = true;
            assignVM.custId = db.Customer.Find(id).CustomerId;
            return View(assignVM);
        }

        // POST: ServiceAssigns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(assignNewspaperVM serviceAssign)
        {
            if (!db.ServiceAssign.Any(m => m.NewspaperId == serviceAssign.NewspaperId && m.CustomerId == serviceAssign.CustomerId))
            {
                if (db.Service.Any(m => m.Id == serviceAssign.NewspaperId && m.TimeBase == "मासिक") && serviceAssign.Duration == 365)
                {
                    serviceAssign.EndedDate = serviceAssign.PaperDispatchDate.AddDays(serviceAssign.Duration - 30);

                }
                else
                {
                    serviceAssign.EndedDate = serviceAssign.PaperDispatchDate.AddDays(serviceAssign.Duration);

                }

                ServiceAssign objService = new ServiceAssign();

                objService.NepaliDate = serviceAssign.NepaliDate;
                objService.NewspaperId = serviceAssign.NewspaperId;
                objService.SalesManId = serviceAssign.SalesManId;


                objService.PaperDispatchDate = Convert.ToDateTime(serviceAssign.PaperDispatchDate.ToShortDateString());
                objService.status = true;
                objService.UpdatedBy = serviceAssign.UpdatedBy;
                objService.UpdatedDate = serviceAssign.UpdatedDate;
                objService.CreatedBy = serviceAssign.CreatedBy;
                objService.CreatedDate = serviceAssign.CreatedDate;
                objService.CustomerId = serviceAssign.CustomerId;
                objService.Quantity = serviceAssign.Quantity;
                objService.EndedDate = Convert.ToDateTime(serviceAssign.EndedDate.ToShortDateString());

                db.ServiceAssign.Add(objService);

                log.AddActivity("Service assign to customer successfully");
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Newspaper Already Assigned.");
                serviceAssign.Newspapers = new SelectList(db.Service, "Id", "NewsPaperName");
                serviceAssign.Salesmans = new SelectList(db.SalesMan, "Id", "FullName");
                return View(serviceAssign);
            }

        }

        // GET: ServiceAssigns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceAssign serviceAssign = db.ServiceAssign.Find(id);
            if (serviceAssign == null)
            {
                return HttpNotFound();
            }
            return View(serviceAssign);
        }

        // POST: ServiceAssigns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PaperDispatchDate,EndedDate,NepaliDate,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,status,CustomerId,NewspaperId")] ServiceAssign serviceAssign)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceAssign).State = EntityState.Modified;
                db.SaveChanges();
                log.AddActivity("Service assign edited sucessfully");
                return RedirectToAction("Index");
            }
            return View(serviceAssign);
        }

        // GET: ServiceAssigns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceAssign serviceAssign = db.ServiceAssign.Find(id);
            if (serviceAssign == null)
            {
                return HttpNotFound();
            }
            return View(serviceAssign);
        }

        // POST: ServiceAssigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServiceAssign serviceAssign = db.ServiceAssign.Find(id);
            db.ServiceAssign.Remove(serviceAssign);
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
