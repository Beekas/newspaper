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
    [SessionCheck(Role = "SuperAdmin,Supervisor")]
    [ValidateInput(false)]
    public class RouteReportsController : Controller
    {
        private activities log = new activities();

        private NewspaperEntities db = new NewspaperEntities();

        // GET: RouteReports
        public ActionResult Index()
        {
            var routeReports = db.RouteReports.Include(r => r.Route);
            return View(routeReports.ToList());
        }

        public ActionResult RouteReportByDate()
        {
            return View();
        }

        public ActionResult RouteReport(string NepDate, string engdate)
        {

            if (engdate == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DateTime engdatea = Convert.ToDateTime(engdate);
            List<RouteReport> prakashanreport = db.RouteReports.Include(m => m.Groupname).Include(m => m.Route).Where(m => m.Date == engdatea).ToList();
            //List<PurchaseReportVM> purchasevm = new List<PurchaseReportVM>();
            var report = db.RouteReports.FirstOrDefault(m => m.Date == engdatea);

            if (report == null)
            {
                TempData["message"] = "No prakashan for record for this date is created";
                return RedirectToAction("RouteReportByDate");
            }
            else
            {
                IEnumerable<RouteReport> routes = db.RouteReports.Where(m => m.NepaliDate == NepDate);
                List<RouteVM> purchasevm = new List<RouteVM>();
                List<RouteVM> purchasevmtotal = new List<RouteVM>();

                var results = (from p in routes
                               group p by p.RouteId into g
                               select new { g.Key }).ToList();

                ViewBag.Routes = results.Select(x => x.Key).ToArray();
                foreach (var item in results.Select(x => x.Key).ToArray())
                {
                    RouteVM objRep = new RouteVM();
                    objRep.gp_total =routes.Where(m => m.RouteId == item).Sum(m => m.gp_Quantity);
                    objRep.rn_total = routes.Where(m => m.RouteId == item).Sum(m => m.rn_Quantity);
                    objRep.muna_total = routes.Where(m => m.RouteId == item).Sum(m => m.muna_Quantity);
                    objRep.madhu_total = routes.Where(m => m.RouteId == item).Sum(m => m.madhu_Quantity);
                    objRep.yuwa_total = routes.Where(m => m.RouteId == item).Sum(m => m.Yuwa_Quantity);
                    objRep.gp_grandtotal = routes.Sum(m => m.gp_Quantity);
                    objRep.rn_grandtotal = routes.Sum(m => m.rn_Quantity);
                    objRep.muna_grandtotal = routes.Sum(m => m.muna_Quantity);
                    objRep.madhu_grandtotal = routes.Sum(m => m.madhu_Quantity);
                    objRep.Yuwa_grandtotal = routes.Sum(m => m.Yuwa_Quantity);
                    objRep.RouteId =Convert.ToInt32(item);
                    purchasevmtotal.Add(objRep);
                }


                foreach (var item in routes)
                {
                    RouteVM objRep = new RouteVM();
                    objRep.AgentName = item.Groupname.AgentName;
                    objRep.Id = item.Id;
                    objRep.Transport = item.Groupname.Transport;
                    objRep.RouteName = item.Route.RouteName;
                    objRep.Address = item.Groupname.Address;
                    objRep.RouteId = Convert.ToInt32(item.RouteId);
                    objRep.gp_Quantity = item.gp_Quantity;
                    objRep.rn_Quantity = item.rn_Quantity;
                    objRep.muna_Quantity = item.muna_Quantity;
                    objRep.madhu_Quantity = item.madhu_Quantity;
                    objRep.Yuwa_Quantity = item.Yuwa_Quantity;
                    objRep.NepaliDate = item.NepaliDate;
                    objRep.Date = item.Date;
                    objRep.gp_total = purchasevmtotal.FirstOrDefault(m => m.RouteId == item.RouteId).gp_total;
                    objRep.rn_total = purchasevmtotal.FirstOrDefault(m => m.RouteId == item.RouteId).rn_total;
                    objRep.muna_total = purchasevmtotal.FirstOrDefault(m => m.RouteId == item.RouteId).muna_total;
                    objRep.madhu_total = purchasevmtotal.FirstOrDefault(m => m.RouteId == item.RouteId).madhu_total;
                    objRep.yuwa_total = purchasevmtotal.FirstOrDefault(m => m.RouteId == item.RouteId).yuwa_total;
                    objRep.gp_grandtotal = purchasevmtotal.FirstOrDefault(m => m.RouteId == item.RouteId).gp_grandtotal;
                    objRep.rn_grandtotal = purchasevmtotal.FirstOrDefault(m => m.RouteId == item.RouteId).rn_total;
                    objRep.muna_grandtotal = purchasevmtotal.FirstOrDefault(m => m.RouteId == item.RouteId).muna_grandtotal;
                    objRep.madhu_grandtotal = purchasevmtotal.FirstOrDefault(m => m.RouteId == item.RouteId).madhu_grandtotal;
                    objRep.Yuwa_grandtotal = purchasevmtotal.FirstOrDefault(m => m.RouteId == item.RouteId).Yuwa_grandtotal;
                    purchasevm.Add(objRep);

                }
                log.AddActivity("Route report printed successfully");
                return View(purchasevm);
                
            }


        }

        public ActionResult EditByDate()
        {

            return View();
        }
        public ActionResult CreateByDate()
        {

            return View();
        }




        // GET: RouteReports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RouteReport routeReport = db.RouteReports.Find(id);
            if (routeReport == null)
            {
                return HttpNotFound();
            }
            return View(routeReport);
        }

        // GET: RouteReports/Create
        public ActionResult Create(string engdate, string NepDate)
        {
            if (engdate == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DateTime engdatea = Convert.ToDateTime(engdate);
            if(db.RouteReports.Any(m=>m.Date==engdatea.Date))
            {
                TempData["message"] = " Production Record for this date is already stored please edit this record here";
                return RedirectToAction("EditByDate");
            }
            List<RouteVM> purchasevm = new List<RouteVM>();
            if (!db.RouteReports.Any())
            {
                var resultss = (from p in db.groupNames
                                group p by p.RouteId into g
                                select new { g.Key }).ToList();

                ViewBag.Routes = resultss.Select(x => x.Key).ToArray();

                foreach (var item in db.groupNames.Include(m => m.Route))
                {
                    RouteVM objRep = new RouteVM();
                    objRep.AgentName = item.AgentName;
                    objRep.Id = item.Id;
                    objRep.Transport = item.Transport;
                    objRep.RouteName = item.Route.RouteName;
                    objRep.Address = item.Address;
                    objRep.RouteId = Convert.ToInt32(item.RouteId);
                    objRep.AgentId = item.Id;
                    objRep.NepaliDate = NepDate;
                    objRep.Date = Convert.ToDateTime(engdate);
                    objRep.gp_Quantity =  Convert.ToInt32(item.GP_Quantity);
                    objRep.rn_Quantity =  Convert.ToInt32(item.RN_Quantity);
                    objRep.muna_Quantity =  Convert.ToInt32(item.MUNA_Quantity);
                    objRep.madhu_Quantity =  Convert.ToInt32(item.Madhu_Quantity);
                    objRep.Yuwa_Quantity =  Convert.ToInt32(item.Yuwa_Quantity);
                    purchasevm.Add(objRep);

                }
                return View(purchasevm);


            }
            else
            {
                DateTime max = db.RouteReports.Max(m => m.Date).Date;

                List<RouteReport> prakashanreport = db.RouteReports.Include(m => m.Groupname).Include(m => m.Route).Where(m => m.Date == max).ToList();
                //List<PurchaseReportVM> purchasevm = new List<PurchaseReportVM>();

                var report = db.RouteReports.Where(m => m.Date == max);


                IEnumerable<RouteReport> routes = db.RouteReports;

                List<int> pastGroup = routes.Select(m => m.AgentId.Value).ToList();
               

                var results = (from p in routes
                               group p by p.RouteId into g
                               select new { g.Key }).ToList();

                

                foreach (var item in prakashanreport)
                {
                    RouteVM objRep = new RouteVM();
                    objRep.AgentName = item.Groupname.AgentName;
                    objRep.Id = item.Id;
                    objRep.Transport = item.Groupname.Transport;
                    objRep.RouteName = item.Route.RouteName;
                    objRep.Address = item.Groupname.Address;
                    objRep.NepaliDate = NepDate;
                    objRep.Date = Convert.ToDateTime(engdate);
                    objRep.RouteId = Convert.ToInt32(item.RouteId);
                    objRep.gp_Quantity = item.gp_Quantity;
                    objRep.rn_Quantity = item.rn_Quantity;
                    objRep.muna_Quantity = item.muna_Quantity;
                    objRep.madhu_Quantity = item.madhu_Quantity;
                    objRep.Yuwa_Quantity = item.Yuwa_Quantity;
                    //objRep.NepaliDate = item.NepaliDate;
                    //objRep.Date = item.Date;
                    objRep.AgentId = item.Groupname.Id;
                    purchasevm.Add(objRep);

                }

                List<int> presentGroup = db.groupNames.Select(m => m.Id).ToList();
              
                IEnumerable<int> remainGroup = presentGroup.Except(pastGroup);

                var uniqroutes = (from p in db.groupNames
                               group p by p.RouteId into g
                               select new { g.Key }).ToList();


                ViewBag.Routes =uniqroutes.Select(m=>m.Key).ToArray();

                foreach (int item in remainGroup)
                {
                    GroupName grp = db.groupNames.Include(m => m.Route).FirstOrDefault(m=>m.Id==item);
                    RouteVM objRep = new RouteVM();
                    objRep.AgentName = grp.AgentName;
                    objRep.Id = item;
                    objRep.AgentId =item;
                    objRep.Transport = grp.Transport;
                    objRep.NepaliDate = NepDate;
                    objRep.Date = Convert.ToDateTime(engdate);
                    objRep.RouteName = grp.Route.RouteName;
                    objRep.gp_Quantity = Convert.ToInt32(grp.GP_Quantity);
                    objRep.rn_Quantity = Convert.ToInt32(grp.RN_Quantity);
                    objRep.muna_Quantity = Convert.ToInt32(grp.MUNA_Quantity);
                    objRep.madhu_Quantity = Convert.ToInt32(grp.Madhu_Quantity);
                    objRep.Yuwa_Quantity = Convert.ToInt32(grp.Yuwa_Quantity);
                    objRep.GroupId = grp.Id.ToString();
                    objRep.Address = grp.Address;
                    objRep.RouteId = Convert.ToInt32(grp.RouteId);

                    purchasevm.Add(objRep);
                }


                //var resultss = (from p in db.groupNames
                //                group p by p.RouteId into g
                //                select new { g.Key }).ToList();

               
                //foreach (var item in db.groupNames.Include(m => m.Route))
                //{
                //    RouteVM objRep = new RouteVM();
                //    objRep.AgentName = item.AgentName;
                //    objRep.Id = item.Id;
                //    objRep.Transport = item.Transport;
                //    objRep.RouteName = item.Route.RouteName;
                //    objRep.Address = item.Address;
                //    objRep.RouteId = Convert.ToInt32(item.RouteId);
                //    purchasevm.Add(objRep);

                //}



                return View(purchasevm);
            }
        }

        // POST: RouteReports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(List<RouteVM> routes, FormCollection col)
        {
            if (ModelState.IsValid)
            {

                DateTime date = Convert.ToDateTime(col["EnglishDate"]);
                string Nepdate = col["NepaliDate"].ToString();
                var report = db.RouteReports.FirstOrDefault(m => m.Date == date);
                if (report == null)
                {
                    foreach (var item in routes)
                    {
                        RouteReport rep = new RouteReport();
                        rep.Route = db.Routes.Find(item.RouteId);
                        rep.RouteId = item.RouteId;
                        
                        rep.NepaliDate = Nepdate;
                        rep.Date = date;
                        rep.gp_Quantity = item.gp_Quantity;
                        rep.rn_Quantity = item.rn_Quantity;
                        rep.muna_Quantity = item.muna_Quantity;
                        rep.madhu_Quantity = item.madhu_Quantity;
                        rep.Yuwa_Quantity = item.Yuwa_Quantity;
                        rep.AgentId = item.AgentId; //GroupId as AgentId
                        rep.Groupname = db.groupNames.Find(item.AgentId);
                        db.RouteReports.Add(rep);
                        db.SaveChanges();
                    }
                    log.AddActivity("Route report inserted successfully");
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = " Production Record for this date is already stored please edit this record here";
                    return RedirectToAction("EditByDate");
                }

                
            }
            var results = (from p in db.groupNames
                           group p by p.RouteId into g
                           select new { g.Key }).ToList();

            ViewBag.Routes = results.Select(x => x.Key).ToArray();


            return View(routes);
        }

        // GET: RouteReports/Edit/5
        public ActionResult Edit(string engdate,string NepDate)
        {
            if (engdate == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DateTime engdatea = Convert.ToDateTime(engdate);
            List<RouteReport> prakashanreport = db.RouteReports.Include(m=>m.Groupname).Include(m=>m.Route).Where(m => m.Date == engdatea).ToList();
            //List<PurchaseReportVM> purchasevm = new List<PurchaseReportVM>();
            var report = db.RouteReports.FirstOrDefault(m => m.Date == engdatea);

            if (report == null)
            {
                TempData["message"] = "No prakashan for record for this date is created";
                return RedirectToAction("EditByDate");


            }
            else {
                IEnumerable<RouteReport> routes = db.RouteReports.Where(m => m.NepaliDate == NepDate);
                List<RouteVM> purchasevm = new List<RouteVM>();

                var results = (from p in routes
                               group p by p.RouteId into g
                               select new { g.Key }).ToList();

                ViewBag.Routes = results.Select(x => x.Key).ToArray();

                foreach (var item in routes)
                {
                    RouteVM objRep = new RouteVM();
                    objRep.AgentName =item.Groupname.AgentName;
                    objRep.Id = item.Id;
                    objRep.Transport = item.Groupname.Transport;
                    objRep.RouteName = item.Route.RouteName;
                    objRep.Address = item.Groupname.Address;
                    objRep.RouteId = Convert.ToInt32(item.RouteId);
                    objRep.gp_Quantity = item.gp_Quantity;
                    objRep.rn_Quantity = item.rn_Quantity;
                    objRep.muna_Quantity = item.muna_Quantity;
                    objRep.madhu_Quantity = item.madhu_Quantity;
                    objRep.Yuwa_Quantity = item.Yuwa_Quantity;
                    objRep.NepaliDate = item.NepaliDate;
                    objRep.Date = item.Date;
                    purchasevm.Add(objRep);

                }
                //purchasevm.Group =db.prakashanGroups.ToList();
                return View(purchasevm);



              //  return RedirectToAction("create");
            }

          
            //{
            //    if (id == null)
            //    {
            //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //    }
            //    RouteReport routeReport = db.RouteReports.Find(id);
            //    if (routeReport == null)
            //    {
            //        return HttpNotFound();
            //    }
            //    ViewBag.RouteId = new SelectList(db.Routes, "Id", "RouteId", routeReport.RouteId);
            //    return View(routeReport);
        }

        // POST: RouteReports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(List<RouteVM> routes, FormCollection col)
        {

            if (ModelState.IsValid)
            {
                foreach (var item in routes)
                {
                    RouteReport rep = db.RouteReports.Find(item.Id);
                    rep.Route = db.Routes.Find(item.RouteId);
                    rep.RouteId = item.RouteId;
                    rep.NepaliDate = col["NepaliDate"].ToString();
                    rep.Date = Convert.ToDateTime(col["EnglishDate"].ToString());
                    rep.gp_Quantity = item.gp_Quantity;
                    rep.rn_Quantity = item.rn_Quantity;
                    rep.muna_Quantity = item.muna_Quantity;
                    rep.madhu_Quantity = item.madhu_Quantity;
                    rep.Yuwa_Quantity = item.Yuwa_Quantity;
                    rep.AgentId = item.Id; //GroupId as AgentId
                   // db.RouteReports.Add(rep);
                    db.SaveChanges();
                }
                log.AddActivity("Route report edited succesfully");
                return RedirectToAction("Index");
            }

            var results = (from p in db.groupNames
                           group p by p.RouteId into g
                           select new { g.Key }).ToList();

            ViewBag.Routes = results.Select(x => x.Key).ToArray();


            return View(routes);
            //if (ModelState.IsValid)
            //{
            //    db.Entry(routeReport).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.RouteId = new SelectList(db.Routes, "Id", "RouteId", routeReport.RouteId);
            //return View(routeReport);
        }

        // GET: RouteReports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RouteReport routeReport = db.RouteReports.Find(id);
            if (routeReport == null)
            {
                return HttpNotFound();
            }
            return View(routeReport);
        }

        // POST: RouteReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RouteReport routeReport = db.RouteReports.Find(id);
            db.RouteReports.Remove(routeReport);
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
