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
    public class PrakashanreportsController : Controller
    {
        private NewspaperEntities db = new NewspaperEntities();
        private activities log = new activities();

        // GET: Prakashanreports
        public ActionResult Index()
        {


            var group = db.prakashanreports.Include(a => a.PrakashanGroup);
            return View(group.ToList());
            //return View(db.prakashanreports.ToList());
        }

        public ActionResult EditByDate()
        {


            return View();
            //return View(db.prakashanreports.ToList());
        }

        // GET: Prakashanreports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prakashanreport prakashanreport = db.prakashanreports.Find(id);
            if (prakashanreport == null)
            {
                return HttpNotFound();
            }
            return View(prakashanreport);
        }

        // GET: Prakashanreports/Create
        public ActionResult Create()
        {
            //ViewBag.GroupId = new SelectList(db.prakashanGroups, "Id", "GroupName");
            ViewBag.NewspaperName = new SelectList(db.Service, "Id", "NewsPaperName");
            var prakasangroup = db.prakashanGroups.ToList();
            List<PurchaseReportVM> purchasevm = new List<PurchaseReportVM>();
            if (!db.prakashanreports.Any())
            {
                foreach (var item in db.prakashanGroups)
                {
                    PurchaseReportVM objRep = new PurchaseReportVM();
                    objRep.GroupName = item.GroupName;
                    objRep.Id = item.Id;
                    objRep.GroupId = item.Id.ToString();
                    purchasevm.Add(objRep);

                }
            }
            else
            {
                DateTime max = db.prakashanreports.Max(m => m.Date).Date;
                List<Prakashanreport> prakashanreport = db.prakashanreports.Include(m => m.PrakashanGroup).Where(m => m.Date == max).ToList();
                //List<PurchaseReportVM> purchasevm = new List<PurchaseReportVM>();

                var report = db.prakashanreports.Where(m => m.Date == max);


                IEnumerable<Prakashanreport> routes = db.prakashanreports;
                foreach (var item1 in prakasangroup)
                {

                    var filtered = prakashanreport.Where(m => m.GroupId == item1.Id);
                    if (filtered.Any())
                    {
                        foreach (var item in filtered)
                        {
                            PurchaseReportVM objRep = new PurchaseReportVM();
                            objRep.GroupName = db.prakashanGroups.Find(item.GroupId).GroupName;
                            objRep.Date = item.Date;
                            objRep.GroupId = item.GroupId.ToString();
                            objRep.NepaliDate = item.NepaliDate;
                            objRep.gp_Total = item.gp_Total;
                            objRep.rn_Total = item.rn_Total;
                            objRep.Remarks = item.Remarks;
                            objRep.Edit_ID = item.Id;
                            purchasevm.Add(objRep);
                        }
                    }
                    else
                    {
                        PurchaseReportVM objRep = new PurchaseReportVM();
                        objRep.GroupName = item1.GroupName;
                        objRep.Id = item1.Id;
                        objRep.GroupId = item1.Id.ToString();
                        purchasevm.Add(objRep);
                    }
                }
        }
                //purchasevm.Group =db.prakashanGroups.ToList();
                return View(purchasevm);
    }

    // POST: Prakashanreports/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(List<PurchaseReportVM> prakashanreport, FormCollection col)
    {
        DateTime date = Convert.ToDateTime(col["EnglishDate"]);
        string Nepdate = col["NepaliDate"].ToString();
        var report = db.prakashanreports.FirstOrDefault(m => m.Date == date);
        if (report == null)
        {
            PurchaseReportVM purchasevm = new PurchaseReportVM();
            foreach (var item in prakashanreport)
            {
                Prakashanreport objreport = new Prakashanreport();
                objreport.NepaliDate = Nepdate;
                objreport.Date = date;
                objreport.GroupId = Convert.ToInt32(item.GroupId);
                objreport.PrakashanGroup = db.prakashanGroups.Find(objreport.GroupId);
                objreport.gp_Total = item.gp_Total;
                objreport.rn_Total = item.rn_Total;
                objreport.Remarks = item.Remarks;
                db.prakashanreports.Add(objreport);
                db.SaveChanges();
            }
            TempData["message"] = "Production record inserted sucessfully";
                log.AddActivity("Production report Successfully Inserted");
            return RedirectToAction("Index");
        }
        else
        {
            TempData["message"] = " Production Record for this date is already stored please edit this record here";
            return RedirectToAction("EditByDate");
        }


    }

    // GET: Prakashanreports/Edit/5
    public ActionResult Edit(string engdate)
    {
        if (engdate == "")
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        DateTime engdatea = Convert.ToDateTime(engdate);
        List<Prakashanreport> prakashanreport = db.prakashanreports.Where(m => m.Date == engdatea).ToList();
        List<PurchaseReportVM> purchasevm = new List<PurchaseReportVM>();
        var report = db.prakashanreports.FirstOrDefault(m => m.Date == engdatea);

        if (report == null)
        {
            TempData["message"] = "No prakashan for record for this date is created";
            return RedirectToAction("EditByDate");


        }
        else
        {
            foreach (var item in prakashanreport)
            {
                PurchaseReportVM objRep = new PurchaseReportVM();
                objRep.GroupName = db.prakashanGroups.Find(item.GroupId).GroupName;
                objRep.Date = item.Date;
                objRep.NepaliDate = item.NepaliDate;
                objRep.gp_Total = item.gp_Total;
                objRep.rn_Total = item.rn_Total;
                objRep.Remarks = item.Remarks;
                objRep.Edit_ID = item.Id;
                purchasevm.Add(objRep);

            }
        }

        if (prakashanreport == null)
        {
            return HttpNotFound();
        }
        return View(purchasevm);
    }

    // POST: Prakashanreports/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(List<PurchaseReportVM> prakashanreport)
    {
        if (ModelState.IsValid)
        {
            PurchaseReportVM purchasevm = new PurchaseReportVM();
            foreach (var item in prakashanreport)
            {
                Prakashanreport objreport = db.prakashanreports.Find(item.Edit_ID);


                objreport.gp_Total = item.gp_Total;
                objreport.rn_Total = item.rn_Total;
                objreport.Remarks = item.Remarks;

                db.SaveChanges();
            }
            TempData["message"] = "Production Report is Sucessfully Edited";
                log.AddActivity("Production report edited successfully");
            return RedirectToAction("Index");

        }
        return View(prakashanreport);
    }

    // GET: Prakashanreports/Delete/5
    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        Prakashanreport prakashanreport = db.prakashanreports.Find(id);
        if (prakashanreport == null)
        {
            return HttpNotFound();
        }
        return View(prakashanreport);
    }

    // POST: Prakashanreports/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        Prakashanreport prakashanreport = db.prakashanreports.Find(id);
        db.prakashanreports.Remove(prakashanreport);
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

    public ActionResult SelectDate()
    {
        return View();

    }

    public ActionResult ProductionReport(string engdate)
    {
        DateTime days = Convert.ToDateTime(engdate);
        DateTime subsdays = days.AddDays(-1);
        List<Prakashanreport> prakashanreport = db.prakashanreports.Where(m => m.Date == days).ToList();
        List<Prakashanreport> prakashanreportyes = db.prakashanreports.Where(m => m.Date == subsdays).ToList();
        List<ProductionReportvm> productiovm = new List<ProductionReportvm>();
        var report = db.prakashanreports.FirstOrDefault(m => m.Date == days);

        if (report != null)
        {

            foreach (var item in prakashanreport)
            {



                ProductionReportvm objRep = new ProductionReportvm();

                objRep.groupname = db.prakashanGroups.Find(item.GroupId).GroupName;
                objRep.datetoday = ADTOBS.EngToNep(days).ToString();
                objRep.dateyes = ADTOBS.EngToNep(subsdays).ToString();

                objRep.gptoday = item.gp_Total;
                if (prakashanreportyes.Any(m => m.GroupId == item.GroupId))
                {
                    objRep.gpyes = prakashanreportyes.FirstOrDefault(m => m.GroupId == item.GroupId).gp_Total;
                    objRep.rnyes = prakashanreportyes.FirstOrDefault(m => m.GroupId == item.GroupId).rn_Total;
                }

                objRep.gpdiff = objRep.gptoday - objRep.gpyes;
                objRep.rntoday = item.rn_Total;

                objRep.rndiff = objRep.rntoday - objRep.rnyes;

                productiovm.Add(objRep);
            }
            ProductionReportvm objReport = new ProductionReportvm();
            objReport.gptodaytotal = productiovm.Sum(m => m.gptoday);
            objReport.gpyestotal = productiovm.Sum(m => m.gpyes);
            objReport.gpdifftotal = productiovm.Sum(m => m.gpdiff);
            objReport.rntodaytotal = productiovm.Sum(m => m.rntoday);
            objReport.rnyestotal = productiovm.Sum(m => m.rnyes);
            objReport.rndifftotal = productiovm.Sum(m => m.rndiff);
            productiovm.Add(objReport);
                log.AddActivity("Production report printed successfully");
            return View(productiovm);
        }
        else
        {
            TempData["message"] = "There is no record for this date";
            return RedirectToAction("SelectDate");
        }

    }
}
}
