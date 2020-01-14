
using Newspaper.Filters;
using Newspaper.Models;
using Newspaper.Models.DAL;
using Newspaper.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Newspaper.Controllers
{
    [SessionCheck(Role = "SuperAdmin,Supervisor")]
    public class ReportController : Controller
    {
        private activities log = new activities();

        private NewspaperEntities db = new NewspaperEntities();
        // GET: Report
        public ActionResult Index()
        {
            var CustomerList = db.Customer.ToList();

            return View(CustomerList);
        }

        public ActionResult ExpiredReport()
        {

            var Newspaper = (from p in db.ServiceAssign
                             from c in db.Customer
                             from s in db.SalesMan
                             from e in db.Service

                             where p.CustomerId == c.Id && p.SalesManId == s.Id && p.NewspaperId == e.Id && p.EndedDate <= DateTime.Now
                             select new
                             {
                                 serviceAssign = p,
                                 customer = c,
                                 salesman = s,
                                 newspaper = e
                             }).ToList();

            List<CounterVM> listCounter = new List<CounterVM>();

            for (int i = 0; i < Newspaper.ToList().Count; i++)
            {
                CounterVM objcounter = new CounterVM();
                objcounter.ServiceAssign = Newspaper[i].serviceAssign;
                objcounter.Customer = Newspaper[i].customer;
                objcounter.NewsPaper = Newspaper[i].newspaper;
               
                
                objcounter.SalesMan = Newspaper[i].salesman;
                objcounter.enddate = ADTOBS.EngToNep(Newspaper[i].serviceAssign.EndedDate).ToString();
                listCounter.Add(objcounter);
                // AddedCustomerVM.Add(new AddedCustomerVM { NewsPaperName = Newspaper[i].NewspaperName, NepaliDate = AddedDate, SalesMan = Newspaper[i].SalesMan/*, NewsPaperName =Newspaper[i].NewspaperName*/, ReportDate = Newspaper[i].date, FirstName = Newspaper[i].FirstName, Address = Newspaper[i].Address, MPhone = Newspaper[i].MPhone, PaperDispatchDate = Newspaper[i].PaperDispatchDate, EndedDate = Newspaper[i].EndedDate, CustomerId = Newspaper[i].CustomerId.ToString(), branch = "All Branches" });

            }


            // var Count = db.ServiceAssign.Where(t => t.EndedDate <= DateTime.Now).ToList();
            if (Newspaper.Count == 0)
            {
                TempData["message"] = "No one is expired";
                return RedirectToAction("Report", "DayWiseCount");
            }
            log.AddActivity("Expired Customers reported printed successfully");
            return View(listCounter);
        }

        //start

        public ActionResult SelectRewNewspaper()
        {

            ViewBag.ServiceId = new SelectList(db.Service, "Id", "NewsPaperName");
            return View();
        }

        public ActionResult RenewCustomerReport(List<AddedCustomerVM> AddedCustomerVM)
        {
            return RedirectToAction("SelectRewNewspaper");
        }
        [HttpPost]
        public ActionResult RenewCustomerReport(FormCollection col)
        {
            List<AddedCustomerVM> AddedCustomerVM = new List<AddedCustomerVM>();
            int ServiceId = Convert.ToInt32(col["ServiceId"]);

            string AddedDate = col["NepDate"].ToString();
            DateTime date = Convert.ToDateTime(col["RegisterDate"]);

            NewspaperEntities db = new NewspaperEntities();
            {
                var Newspaper = (from p in db.ServiceAssign
                                 from c in db.Customer
                                 from s in db.SalesMan
                                 from e in db.Service
                                     // replace  && p.CustomerId == c.Id && p.SalesManId == s.Id && p.NewspaperId == e.Id 
                                     //where p.NewspaperId == e.Id && p.CreatedDate == date && p.CustomerId.ToString() == c.CustomerId && p.SalesManId == s.Id && p.NewspaperId == ServiceId
                                 where p.NewspaperId == ServiceId && p.UpdatedDate == date && p.PaymentType == true && p.CustomerId == c.Id && p.SalesManId == s.Id && p.NewspaperId == e.Id
                                 select new
                                 {
                                     serviceId = p.NewspaperId,
                                     FirstName = c.FirstName,
                                    quantity = p.Quantity,
                                     NepaliDate = p.NepaliDate,
                                     SalesMan = s.FullName,
                                     MPhone = c.MPhone,
                                     Address = c.Address,
                                     PaperDispatchDate = p.PaperDispatchDate,
                                     EndedDate = p.EndedDate,
                                     
                                     NewspaperName = e.NewsPaperName,
                                     date = p.UpdatedDate,
                                     CustomerId = c.CustomerId
                                 }).ToList();

                if (Newspaper == null || Newspaper.Count == 0)
                {
                    TempData["message"] = "No record found.";
                    return RedirectToAction("SelectRewNewspaper");
                }
                for (int i = 0; i < Newspaper.ToList().Count; i++)
                {
                    if (Newspaper[i].CustomerId == null)
                    {
                        AddedCustomerVM.Add(new AddedCustomerVM { dispatch = ADTOBS.EngToNep(Newspaper[i].PaperDispatchDate).ToString(), enddate = ADTOBS.EngToNep(Newspaper[i].EndedDate).ToString(), NewsPaperName = Newspaper[i].NewspaperName, NepaliDate = AddedDate, SalesMan = Newspaper[i].SalesMan/*, NewsPaperName =Newspaper[i].NewspaperName*/, ReportDate = Newspaper[i].date, FirstName = Newspaper[i].FirstName, quantity =Convert.ToInt32(Newspaper[i].quantity).ToString(), Address = Newspaper[i].Address, MPhone = Newspaper[i].MPhone, PaperDispatchDate = Newspaper[i].PaperDispatchDate, EndedDate = Newspaper[i].EndedDate, branch = "All Branches" });
                    }
                    else
                    {



                        AddedCustomerVM.Add(new AddedCustomerVM
                        {
                            dispatch = ADTOBS.EngToNep(Newspaper[i].PaperDispatchDate).ToString(),
                            enddate = ADTOBS.EngToNep(Newspaper[i].EndedDate).ToString(),
                            NewsPaperName = Newspaper[i].NewspaperName,
                            quantity = Convert.ToInt32(Newspaper[i].quantity).ToString(),
                            NepaliDate = AddedDate,
                            SalesMan = Newspaper[i].SalesMan/*, NewsPaperName =Newspaper[i].NewspaperName*/,
                            ReportDate = Newspaper[i].date,
                            FirstName = Newspaper[i].FirstName,
                            Address = Newspaper[i].Address,
                            MPhone = Newspaper[i].MPhone,
                            PaperDispatchDate = Newspaper[i].PaperDispatchDate,
                            EndedDate = Newspaper[i].EndedDate,
                            CustomerId = Newspaper[i].CustomerId.ToString(),
                        });

                    }
                }
                if (AddedCustomerVM == null)
                {
                    TempData["message"] = "No record found.";
                    return RedirectToAction("SelectRewNewspaper");
                }
                try
                {

                    String Operation = "Renew Customer Report Printed";
                    db.ActivityLog.Add(new ActivityLog
                    {

                        Operation = Operation,
                        CreatedBy = Session["userEmail"].ToString(),
                        CreatedDate = DateTime.Now

                    });
                    db.SaveChanges();
                }
                catch
                {
                    ViewBag.ErrorMessage = "Email sending failed";
                }
                return View(AddedCustomerVM);
            }


        }


        //end








        public ActionResult SelectNewspaper()
        {

            ViewBag.ServiceId = new SelectList(db.Service, "Id", "NewsPaperName");
            return View();
        }

        public ActionResult AddedCustomerReport(List<AddedCustomerVM> AddedCustomerVM)
        {
            return RedirectToAction("SelectNewspaper");
        }
        [HttpPost]
        public ActionResult AddedCustomerReport(FormCollection col)
        {
            List<AddedCustomerVM> AddedCustomerVM = new List<AddedCustomerVM>();
            int ServiceId = Convert.ToInt32(col["ServiceId"]);

            string AddedDate = col["NepDate"].ToString();
            DateTime date = Convert.ToDateTime(col["RegisterDate"]);

            NewspaperEntities db = new NewspaperEntities();
            {
                var Newspaper = (from p in db.ServiceAssign
                                 from c in db.Customer
                                 from s in db.SalesMan
                                 from e in db.Service
                                     // replace  && p.CustomerId == c.Id && p.SalesManId == s.Id && p.NewspaperId == e.Id 
                                     //where p.NewspaperId == e.Id && p.CreatedDate == date && p.CustomerId.ToString() == c.CustomerId && p.SalesManId == s.Id && p.NewspaperId == ServiceId
                                 where p.NewspaperId == ServiceId && p.UpdatedDate == date && p.PaymentType == false && p.CustomerId == c.Id && p.SalesManId == s.Id && p.NewspaperId == e.Id
                                 select new
                                 {
                                     serviceId = p.NewspaperId,
                                     FirstName = c.FirstName,
                                     NepaliDate = p.NepaliDate,
                                     SalesMan = s.FullName,
                                     quantity = p.Quantity,
                                     MPhone = c.MPhone,
                                     Address = c.Address,
                                     PaperDispatchDate = p.PaperDispatchDate,
                                     EndedDate = p.EndedDate,
                                     NewspaperName = e.NewsPaperName,
                                     date = c.RegisterDate,
                                     CustomerId = c.CustomerId
                                 }).ToList();

                if (Newspaper == null || Newspaper.Count == 0)
                {
                    TempData["message"] = "No record found.";
                    return RedirectToAction("SelectNewspaper");
                }
                for (int i = 0; i < Newspaper.ToList().Count; i++)
                {
                    if (Newspaper[i].CustomerId == null)
                    {

                        AddedCustomerVM.Add(new AddedCustomerVM
                        {
                            dispatch = ADTOBS.EngToNep(Newspaper[i].PaperDispatchDate).ToString(),
                            enddate = ADTOBS.EngToNep(Newspaper[i].EndedDate).ToString(),
                            NewsPaperName = Newspaper[i].NewspaperName,
                            NepaliDate = AddedDate,
                            SalesMan = Newspaper[i].SalesMan,
                            quantity = Convert.ToInt32(Newspaper[i].quantity).ToString(),
                            ReportDate = Newspaper[i].date,
                            FirstName = Newspaper[i].FirstName,
                            Address = Newspaper[i].Address,
                            MPhone = Newspaper[i].MPhone,
                            PaperDispatchDate = Newspaper[i].PaperDispatchDate,
                            EndedDate = Newspaper[i].EndedDate,

                            branch = "All Branches"
                        });
                    }
                    else
                    {
                        AddedCustomerVM.Add(new AddedCustomerVM
                        {
                            dispatch = ADTOBS.EngToNep(Newspaper[i].PaperDispatchDate).ToString(),
                            enddate = ADTOBS.EngToNep(Newspaper[i].EndedDate).ToString(),
                            NewsPaperName = Newspaper[i].NewspaperName,
                            quantity = Convert.ToInt32(Newspaper[i].quantity).ToString(),
                            NepaliDate = AddedDate,
                            SalesMan = Newspaper[i].SalesMan,
                            ReportDate = Newspaper[i].date,
                            FirstName = Newspaper[i].FirstName,
                            Address = Newspaper[i].Address,
                            MPhone = Newspaper[i].MPhone,
                            PaperDispatchDate = Newspaper[i].PaperDispatchDate,
                            EndedDate = Newspaper[i].EndedDate,
                            CustomerId = Newspaper[i].CustomerId.ToString(),
                            branch = "All Branches"
                        });
                    }
                }
                if (AddedCustomerVM == null)
                {
                    TempData["message"] = "No record found.";
                    return RedirectToAction("SelectNewspaper");
                }
                try
                {

                    String Operation = "Added Customer Report Printed";
                    db.ActivityLog.Add(new ActivityLog
                    {

                        Operation = Operation,
                        CreatedBy = Session["userEmail"].ToString(),
                        CreatedDate = DateTime.Now

                    });
                    db.SaveChanges();
                }
                catch
                {
                    ViewBag.ErrorMessage = "Email sending failed";
                }
                return View(AddedCustomerVM);
            }


        }



        public ActionResult SelectDate()
        {
            //List<Branch> listBranch = db.Branch.ToList();
            //listBranch.Add(new Branch { BranchId = 0, BranchName = "All Branches" });
            //ViewBag.BranchId = new SelectList(listBranch, "BranchId", "BranchName");
            return View();
        }
        public ActionResult ExpiredCustomerBydate(List<AddedCustomerVM> AddedCustomerVM)
        {
            return RedirectToAction("SelectDate");
        }
        [HttpPost]
        public ActionResult ExpiredCustomerBydate(FormCollection col)
        {

            List<AddedCustomerVM> AddedCustomerVM = new List<AddedCustomerVM>();
            string EndedDate = col["NepDate"].ToString();
            DateTime date = Convert.ToDateTime(col["EndedDate"]);

            NewspaperEntities db = new NewspaperEntities();

            var Newspaper = (from s in db.ServiceAssign
                             from e in db.Service
                             from d in db.SalesMan
                             from c in db.Customer
                             where s.EndedDate == date && s.CustomerId == c.Id && s.SalesManId == d.Id && s.NewspaperId == e.Id
                             select new
                             {
                                 ServiceId = s.NewspaperId,
                                 FirstName = c.FirstName,
                                 SalesMan = d.FullName,
                                 MPhone = c.MPhone,
                                 Address = c.Address,
                                 NewspaperName = e.NewsPaperName,
                                 date = s.EndedDate,
                                 CustomerId = c.CustomerId
                             }).ToList();






            if (Newspaper == null || Newspaper.Count == 0)
            {
                TempData["message"] = "No record found.";
                return RedirectToAction("SelectDate");
            }
            for (int i = 0; i < Newspaper.ToList().Count; i++)
            {

                AddedCustomerVM.Add(new AddedCustomerVM { enddate = ADTOBS.EngToNep(date).ToString(), EndedDate = date, NepaliDate = EndedDate, SalesMan = Newspaper[i].SalesMan, NewsPaperName = Newspaper[i].NewspaperName, ReportDate = Newspaper[i].date, FirstName = Newspaper[i].FirstName, Address = Newspaper[i].Address, MPhone = Newspaper[i].MPhone, CustomerId = Newspaper[i].CustomerId });

            }
            if (AddedCustomerVM == null)
            {
                TempData["message"] = "No record found.";
                return RedirectToAction("SelectDistributorDate");
            }
            try
            {

                String Operation = "Expired Reported printed";

                db.ActivityLog.Add(new ActivityLog
                {

                    Operation = Operation,
                    CreatedBy = Session["userEmail"].ToString(),
                    CreatedDate = DateTime.Now

                });
                db.SaveChanges();

            }
            catch
            {
                ViewBag.ErrorMessage = "Email sending failed";

            }

            return View(AddedCustomerVM);

        }



        //public List<AddedCustomerVM> getsub(int BranchId, string EndedDate, DateTime date, int branch)
        //{
        //    List<AddedCustomerVM> AddedCustomerVM = new List<AddedCustomerVM>();

        //    NewspaperEntities db = new NewspaperEntities();

        //    var Newspaper = (from p in db.Customer
        //                     join s in db.SalesMan on p.SalesManId equals s.Id
        //                     where p.EndedDate == date && p.BranchId == BranchId
        //                     select new
        //                     {
        //                         ServiceId = p.ServiceId,
        //                         FirstName = p.FirstName,
        //                         MiddleName = p.MiddleName,
        //                         NepaliDate = p.NepaliDate,
        //                         SalesMan = s.FullName,
        //                         LastName = p.LastName,
        //                         MPhone = p.MPhone,
        //                         Address = p.Address,
        //                         NewspaperName = p.Service.NewsPaperName,
        //                         date = p.EndedDate,
        //                         CustomerId = p.CustomerId

        //                     }).ToList();/* db.Customer.Include("ServiceId").Include("RegisterDate").FirstOrDefault(m => m.ServiceId == ServiceId && m.RegisterDate == date);*/

        //    var Paper = db.Customer.Where(m => m.EndedDate == date && m.BranchId == BranchId).ToList();
        //    var objbranch = db.Branch.Find(BranchId);
        //    if (Newspaper == null || Newspaper.Count == 0)
        //    {
        //        TempData["message"] = "No record found.";
        //        return null;
        //    }
        //    for (int i = 0; i < Newspaper.ToList().Count; i++)
        //    {
        //        AddedCustomerVM.Add(new AddedCustomerVM { EndedDate = date, NepaliDate = EndedDate, SalesMan = Newspaper[i].SalesMan, NewsPaperName = Newspaper[i].NewspaperName, ReportDate = Newspaper[i].date, FirstName = Newspaper[i].FirstName, MiddleName = Newspaper[i].MiddleName, LastName = Newspaper[i].LastName, Address = Newspaper[i].Address, MPhone = Newspaper[i].MPhone, CustomerId = Newspaper[i].CustomerId, branch = objbranch.BranchName });
        //        // imagesList.Add(new SelectSalesManVM { NewsPaperName=customer[i].ServiceId, Count = customer[i].Count });
        //    }
        //    try
        //    {

        //        //String Operation = "Expired report Printed";
        //        //db.ActivityLog.Add(new ActivityLog
        //        //{
        //        //    BranchId = BranchId,
        //        //    Operation = Operation,
        //        //    CreatedBy = Session["userEmail"].ToString(),
        //        //    CreatedDate = DateTime.Now

        //        //});
        //        db.SaveChanges();
        //    }
        //    catch
        //    {
        //        ViewBag.ErrorMessage = "Email sending failed";
        //    }


        //    return AddedCustomerVM;
        //}


        public ActionResult SelectDistributorDate()
        {
            //List<Branch> listBranch = db.Branch.ToList();
            //listBranch.Add(new Branch { BranchId = 0, BranchName = "All Branches" });
            //ViewBag.BranchId = new SelectList(listBranch, "BranchId", "BranchName");
            ViewBag.ServiceId = new SelectList(db.Service, "Id", "NewsPaperName");
            return View();
        }

        public ActionResult DistributorAddReport(List<DistributorVM> DistributorVM)
        {
            return RedirectToAction("SelectDistributorDate");
        }

        [HttpPost]
        public ActionResult DistributorAddReport(FormCollection col)
        {

            List<DistributorVM> DistributorVM = new List<DistributorVM>();
            int ServiceId = Convert.ToInt32(col["ServiceId"]);
            string EndedDate = col["NepDate"].ToString();
            DateTime date = Convert.ToDateTime(col["EndedDate"]);
            DateTime yes = date.AddDays(-1);
            NewspaperEntities db = new NewspaperEntities();
            int serviceId = Convert.ToInt32(col["ServiceId"].ToString());

            var objService = db.Service.Find(serviceId);


            //var cus = (from s in db.ServiceAssign
            //           from c in db.Customer
            //           from n in db.Newspaper
            //           where s.NewspaperId == n.Id && s.CustomerId == c.Id &&
            //           select new
            //           {
            //               Customer = c,
            //               ServiceAssign = s,
            //               NewsPaper = n  g.FirstOrDefault(m => m.CustomerId == g.Key).SalesManId
            //           }); // as IEnumerable<CounterVM>;


            var paperAll = db.ServiceAssign.Where(m => m.EndedDate >= date && m.PaperDispatchDate <= date).Where(m => m.NewspaperId == serviceId).GroupBy(n => n.SalesManId)
                                   .Select(g => new { FullName = g.Key, SalesManId = g.Key, Count = g.Sum(m => m.Quantity) }).ToList();

            var PaperTotal = db.ServiceAssign.Where(m => m.EndedDate >= yes && m.PaperDispatchDate <= yes).Where(m => m.NewspaperId == serviceId).GroupBy(n => n.SalesManId)
                                   .Select(g => new { FullName = g.Key, SalesManId = g.Key, Count = g.Sum(m => m.Quantity) }).ToList();
            var substracted = db.ServiceAssign.Where(m => m.EndedDate == date).Where(m => m.NewspaperId == serviceId).GroupBy(n => n.SalesManId)
                                    .Select(g => new { FullName = g.Key, SalesManId = g.Key, Count = g.Sum(m => m.Quantity) }).ToList();

            var added = db.ServiceAssign.Where(m => m.PaperDispatchDate == date).Where(m => m.NewspaperId == serviceId).GroupBy(n => n.SalesManId)
                                   .Select(g => new { FullName = g.Key, SalesManId = g.Key, Count = g.Sum(m => m.Quantity) }).ToList();

            for (int i = 0; i < paperAll.Count; i++)
            {
                int addedInt = 0, substractInt = 0, paperTotalInt = 0;
                if (PaperTotal.FirstOrDefault(m => m.SalesManId == paperAll[i].SalesManId) == null)
                {
                    paperTotalInt = 0;
                }
                else
                {
                    paperTotalInt = PaperTotal.FirstOrDefault(m => m.SalesManId == paperAll[i].SalesManId).Count;
                }

                if (added.FirstOrDefault(m => m.SalesManId == paperAll[i].SalesManId) == null)
                {
                    addedInt = 0;
                }
                else
                {
                    addedInt = added.FirstOrDefault(m => m.SalesManId == paperAll[i].SalesManId).Count;
                }

                if (substracted.FirstOrDefault(m => m.SalesManId == paperAll[i].SalesManId) == null)
                {
                    substractInt = 0;
                }
                else
                {
                    substractInt = substracted.FirstOrDefault(m => m.SalesManId == paperAll[i].SalesManId).Count;
                }


                DistributorVM.Add(new DistributorVM
                {
                    DistributorName = db.SalesMan.Find(paperAll[i].SalesManId).FullName,
                    newspaperName = objService.NewsPaperName,
                    Quantity = paperTotalInt,

                    Added = addedInt,
                    Subs = substractInt,
                    NepaliDate = EndedDate,
                });

            }
            try
            {
                string operation = "Distributor Report is created";
                db.ActivityLog.Add(new ActivityLog
                {
                    Operation = operation,
                    CreatedBy = Session["userEmail"].ToString(),
                    CreatedDate = DateTime.Now,
                });
                db.SaveChanges();
            }
            catch
            {
                ViewBag.ErrorMessage = "Distributor Report Failed to print";
            }
            if (DistributorVM.Count == 0)
            {
                TempData["message"] = "No record found.";
                return RedirectToAction("SelectDistributorDate");
            }

            return View(DistributorVM);






        }



        //public List<DistributorVM> getDistributer(int BranchId, int ServiceId, string EndedDate, DateTime date, DateTime yes, int serviceId, int branch)
        //{
        //    NewspaperEntities db = new NewspaperEntities();

        //    List<DistributorVM> DistributorVM = new List<DistributorVM>();

        //    var objBranch = db.Branch.Find(BranchId);
        //    var objService = db.Service.Find(serviceId);
        //    var paperAll = db.Customer.Where(m => m.EndedDate >= date && m.PaperDispatchDate <= date && m.BranchId == BranchId).Where(m => m.ServiceId == serviceId).GroupBy(n => n.SaleMan.FullName)
        //                       .Select(g => new { FullName = g.FirstOrDefault(m => m.CustomerId == g.Key).SaleMan.FullName, SalesManId = g.Key, Count = g.Sum(m => m.Qunatity) }).ToList();

        //    var PaperTotal = db.Customer.Where(m => m.EndedDate >= yes && m.PaperDispatchDate <= yes && m.BranchId == BranchId).Where(m => m.ServiceId == serviceId).GroupBy(n => n.SaleMan.FullName)
        //                           .Select(g => new { FullName = g.FirstOrDefault(m => m.CustomerId == g.Key).SaleMan.FullName, SalesManId = g.Key, Count = g.Sum(m => m.Qunatity) }).ToList();
        //    var substracted = db.Customer.Where(m => m.EndedDate == date && m.BranchId == BranchId).Where(m => m.ServiceId == serviceId).GroupBy(n => n.SaleMan.FullName)
        //                            .Select(g => new { FullName = g.FirstOrDefault(m => m.CustomerId == g.Key).SaleMan.FullName, SalesManId = g.Key, Count = g.Sum(m => m.Qunatity) }).ToList();
        //    var added = db.Customer.Where(m => m.PaperDispatchDate == date & m.BranchId == BranchId).Where(m => m.ServiceId == serviceId).GroupBy(n => n.SaleMan.FullName)
        //                           .Select(g => new { FullName = g.FirstOrDefault(m => m.CustomerId == g.Key).SaleMan.FullName, SalesManId = g.Key, Count = g.Sum(m => m.Qunatity) }).ToList();


        //    if (paperAll.Count == 0)
        //    {

        //        List<DistributorVM> objDistributor = new List<DistributorVM>();
        //        return objDistributor;

        //    }
        //    else
        //    {
        //        for (int i = 0; i < paperAll.Count; i++)
        //        {
        //            int addedInt = 0, substractInt = 0, paperTotalInt = 0;
        //            if (PaperTotal.FirstOrDefault(m => m.SalesManId == paperAll[i].SalesManId) == null)
        //            {
        //                paperTotalInt = 0;
        //            }
        //            else
        //            {
        //                paperTotalInt = PaperTotal.FirstOrDefault(m => m.SalesManId == paperAll[i].SalesManId).Count;
        //            }

        //            if (added.FirstOrDefault(m => m.SalesManId == paperAll[i].SalesManId) == null)
        //            {
        //                addedInt = 0;
        //            }
        //            else
        //            {
        //                addedInt = added.FirstOrDefault(m => m.SalesManId == paperAll[i].SalesManId).Count;
        //            }

        //            if (substracted.FirstOrDefault(m => m.SalesManId == paperAll[i].SalesManId) == null)
        //            {
        //                substractInt = 0;
        //            }
        //            else
        //            {
        //                substractInt = substracted.FirstOrDefault(m => m.SalesManId == paperAll[i].SalesManId).Count;
        //            }


        //            DistributorVM.Add(new DistributorVM
        //            {
        //                DistributorName = paperAll[i].SalesManId,
        //                newspaperName = objService.NewsPaperName,
        //                branch = objBranch.BranchName,
        //                Quantity = paperTotalInt,
        //                Added = addedInt,
        //                Subs = substractInt,
        //                NepaliDate = EndedDate,
        //            });
        //        }
        //        if (Session["Category"].ToString() == "SuperAdmin")
        //        {
        //            string operation = "Distributor Report Printed for" + " " + objBranch.BranchName + " " + "Branch";
        //            db.ActivityLog.Add(new ActivityLog
        //            {
        //                Operation = operation,
        //                CreatedBy = Session["userEmail"].ToString(),
        //                CreatedDate = DateTime.Now

        //            });
        //            db.SaveChanges();
        //        }
        //        else
        //        {
        //            string Operation = "Distributor Report Printed";
        //            db.ActivityLog.Add(new ActivityLog
        //            {

        //                BranchId = BranchId,
        //                Operation = Operation,
        //                CreatedBy = Session["userEmail"].ToString(),
        //                CreatedDate = DateTime.Now
        //            });
        //            db.SaveChanges();
        //        }
        //    }
        //    return DistributorVM;
        //}







    }
}
