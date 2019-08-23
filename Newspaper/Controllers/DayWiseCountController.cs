using Newspaper.Filters;
using Newspaper.Models;
using Newspaper.Models.DAL;
using Newspaper.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace Newspaper.Controllers
{
    [SessionCheck(Role ="SuperAdmin,Supervisor")]
    
    [ValidateInput(false)]
    public class DayWiseCountController : Controller
    {
        private activities log = new activities();

        private NewspaperEntities db = new NewspaperEntities();

        //public ActionResult Index()
        //{
        //    return View();
        //}
        //public ActionResult Create()
        //{

        //    ViewBag.SalesManId = new SelectList(db.SalesMan, "Id", "FullName");
        //    ViewBag.ServiceId = new SelectList(db.Service, "Id", "NewsPaperName");

        //    var customers = db.Customer.Where(s => s.EndedDate >= DateTime.Now).ToList();
        //    List<CustomerVM> customerVM = new List<CustomerVM>();
        //    foreach (var item in customers)
        //    {
        //        CustomerVM customer = new CustomerVM();
        //        customer.CustomerId = item.Id;
        //        customer.IsPaperDispatched = true;
        //        customer.SalesMan = item.SaleMan.FullName;
        //        customer.NewsPaperName = item.Service.NewsPaperName;
        //        customer.Address = item.Address;
        //        customer.CustomerName = item.FirstName + " " + item.LastName;
        //        customerVM.Add(customer);

        //    }
        //    return View(customerVM);
        //}
        //[HttpPost]

        //public ActionResult GetCustomerList(int salesManId, int serviceId)
        //{
        //    List<CustomerVM> customerVM = new List<CustomerVM>();

        //    var customer = db.Customer.Where(m => m.ServiceId == serviceId && m.SalesManId == salesManId && m.EndedDate >= DateTime.Now).ToList();
        //    foreach (var item in customer)
        //    {
        //        CustomerVM objCustomer = new CustomerVM();
        //        objCustomer.Address = item.Address;
        //        objCustomer.SalesMan = item.SaleMan.FullName;
        //        objCustomer.NewsPaperName = item.Service.NewsPaperName;
        //        objCustomer.CustomerName = item.FirstName + item.LastName;
        //        objCustomer.CustomerId = item.Id;
        //        objCustomer.IsPaperDispatched = true;
        //        customerVM.Add(objCustomer);
        //    }
        //    return Json(customerVM);


        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(List<CustomerVM> customerVM, FormCollection col)
        //{
        //    string count = col["NepDate"].ToString();
        //    DateTime paperDispatchDate = Convert.ToDateTime(col["PaperDispatchDate"]);
        //    int salesManId = Convert.ToInt32(col["SalesManId"]);
        //    int ServiceId = Convert.ToInt32(col["ServiceId"]);
        //    if (db.DayWisePaperDispatch.FirstOrDefault(s => s.SalesManId == salesManId && s.ServiceId == ServiceId && s.PaperDispatchDate == paperDispatchDate) == null)
        //    {
        //        foreach (var item in customerVM)
        //        {
        //            if (item.IsPaperDispatched)
        //            {
        //                DaywisePaperDispatch dayWisePaper = new DaywisePaperDispatch();
        //                dayWisePaper.SalesManId = salesManId;
        //                dayWisePaper.PaperDispatchDate = paperDispatchDate;
        //                dayWisePaper.CustomerId = item.CustomerId;

        //                dayWisePaper.Remarks = col["Remarks"];
        //                dayWisePaper.SubmittedDate = DateTime.Now;
        //                dayWisePaper.SubmittedBy = Session["userEmail"].ToString();
        //                dayWisePaper.ServiceId = ServiceId;

        //                db.DayWisePaperDispatch.Add(dayWisePaper);
        //            }
        //        }
        //        db.SaveChanges();
        //        ViewBag.message = "Record inserted Sucessfully";
        //    }
        //    else
        //    {
        //        ViewBag.errormessage = "Record Already Exist for selected item";
        //    }
        //    ViewBag.SalesManId = new SelectList(db.SalesMan, "Id", "FullName");
        //    ViewBag.ServiceId = new SelectList(db.Service, "Id", "NewsPaperName");

        //    var customers = db.Customer.ToList();
        //    List<CustomerVM> customerVMs = new List<CustomerVM>();
        //    foreach (var item in customers)
        //    {
        //        CustomerVM customer = new CustomerVM();
        //        customer.CustomerId = item.Id;
        //        customer.IsPaperDispatched = true;
        //        customer.SalesMan = item.SaleMan.FullName;
        //        customer.NewsPaperName = item.Service.NewsPaperName;
        //        customer.Address = item.Address;
        //        customer.CustomerName = item.FirstName + " " + item.LastName;
        //        customerVMs.Add(customer);

        //    }
        //    try
        //    {
        //        string Email = Session["userEmail"].ToString();
        //        String Operation = "Daywise Count Created Sucessfully" + " " + Email;
        //        db.ActivityLog.Add(new ActivityLog
        //        {
        //            Operation = Operation,
        //            CreatedBy = Session["userEmail"].ToString(),
        //            CreatedDate = DateTime.Now

        //        });
        //        db.SaveChanges();

        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //    return View(customerVMs);
        //}


        public ActionResult SelectSalesMan()
        {

            ViewBag.SalesManId = new SelectList(db.SalesMan, "Id", "FullName");
            return View();

        }
        [HttpPost]
        public ActionResult CountSalesManReport(FormCollection col)
        {

            List<SelectSalesManVM> SelectSalesManVM = new List<SelectSalesManVM>();
            string NepaliDate = col["NepDate"].ToString();
            int salesManId = Convert.ToInt32(col["SalesManId"]);
            DateTime date = Convert.ToDateTime(col["PaperDispatchDate"]);


            


            NewspaperEntities db = new NewspaperEntities();


            var salesManList = db.SalesMan.ToList();
            foreach (var item in salesManList)
            {
                    var customer = db.ServiceAssign.Where(m => m.EndedDate > date && m.SalesManId == item.Id && m.PaperDispatchDate < date).GroupBy(n => n.NewspaperId)
                                                .Select(g => new { ServiceId = g.Key, Count = g.Sum(m => m.Quantity), newspaperId = g.FirstOrDefault().NewspaperId }).ToList();


                    for (int i = 0; i < customer.ToList().Count; i++)
                    {
                        int newsId = Int32.Parse(customer[i].newspaperId.ToString());
                        string newspapername = db.Service.FirstOrDefault(m => m.Id == newsId).NewsPaperName;
                        SelectSalesManVM.Add(new SelectSalesManVM {SalesManId=item.Id, NepDate = NepaliDate, SalesManName = item.FullName, ReportDate = DateTime.Now, NewsPaperName = newspapername, Count = customer[i].Count, branch = "All Branches" });

                    }

                
            }
            if (SelectSalesManVM.Count == 0)
            {
                TempData["message"] = "No record found.";
                return RedirectToAction("SelectSalesMan");

            }
            try
            {

                string Operation = "Daywise count Report is printed";
                db.ActivityLog.Add(new ActivityLog
                {
                    Operation = Operation,
                    CreatedBy = Session["userName"].ToString(),
                    CreatedDate = DateTime.Now
                });

            }
            catch
            {
                ViewBag.ErrorMessage = "Daywise count Report failed to print";
            }
            log.AddActivity("Gatepass report printed successfully");
            return View(SelectSalesManVM);
        }




        //public List<SelectSalesManVM> getReport(int salesManId, DateTime date, int branch, int BranchId, string NepaliDate)
        //{

        //    List<SelectSalesManVM> SelectSalesManVM = new List<SelectSalesManVM>();
        //    NewspaperEntities db = new NewspaperEntities();
        //    var objBranch = db.Branch.Find(BranchId);
        //    var salesManList = db.SalesMan.ToList();
        //    if (salesManId == 0)
        //    {

        //        salesManList = db.SalesMan.Where(m => m.BranchId == BranchId).ToList();

        //    }
        //    foreach (var item in salesManList)
        //    {

        //        var customer = db.Customer.Where(m => m.EndedDate > date && m.SalesManId == item.Id && m.PaperDispatchDate < date && m.BranchId == BranchId).GroupBy(n => n.Service.NewsPaperName)
        //                                    .Select(g => new { ServiceId = g.Key, Count = g.Sum(m => m.Qunatity) }).ToList();



        //        for (int i = 0; i < customer.ToList().Count; i++)
        //        {
        //            SelectSalesManVM.Add(new SelectSalesManVM { NepDate = NepaliDate, SalesManName = item.FullName, ReportDate = DateTime.Now, NewsPaperName = customer[i].ServiceId, Count = customer[i].Count, branch = objBranch.BranchName });

        //        }


        //    }
        //    try
        //    {
        //        if (Session["Category"].ToString() == "SuperAdmin")
        //        {
        //            string operation = "Daywise Count Report is Printed for" + " " + objBranch.BranchName + " " + "Branch";
        //            db.ActivityLog.Add(new ActivityLog
        //            {
        //                BranchId = BranchId,
        //                Operation = operation,
        //                CreatedBy = Session["userEmail"].ToString(),
        //                CreatedDate = DateTime.Now

        //            });
        //            db.SaveChanges();
        //        }
        //        else
        //        {
        //            string operation = "Daywise Count Report is Printed";
        //            db.ActivityLog.Add(new ActivityLog
        //            {
        //                BranchId = BranchId,
        //                Operation = operation,
        //                CreatedBy = Session["userEmail"].ToString(),
        //                CreatedDate = DateTime.Now

        //            });
        //            db.SaveChanges();
        //        }
        //    }
        //    catch
        //    {
        //        ViewBag.ErrorMessage = "Failed to create Daywise Report";
        //    }

        //    return SelectSalesManVM;
        //}





        //public ActionResult CountSalesManReport(List<SelectSalesManVM> selectSalesManVM)
        //{
        //    return RedirectToAction("SelectSalesMan");

        //}
        public ActionResult Report()
        {
            return View();
        }
    }

}

