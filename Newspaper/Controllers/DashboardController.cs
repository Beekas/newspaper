using Newspaper.Filters;
using Newspaper.Models;
using Newspaper.Models.DAL;
//using PagedList;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Newspaper.Models.ViewModel;


namespace Newspaper.Controllers
{
   // Type customerType = domainAssembly.GetType("Domain.Customer");
   
    //  ADTOBS


    [SessionCheck(Role ="Admin,SuperAdmin,Supervisor")]
    [ValidateInput(false)]
    public class DashboardController : Controller
    {
        // GET: Dashboard

        NewspaperEntities db = new NewspaperEntities();
        public ActionResult Index()//string sortOrder,string CurrentSort, string currentFilter, string searchString, int? page)
        {
            
          
                var Cus = (from m in db.ServiceAssign
                          group m by new { m.CustomerId }
                                   into mygroup
                           select mygroup.FirstOrDefault()).ToList();
                var sale = db.SalesMan.ToList();
                var Custom = db.Customer.ToList();
            var news = db.Service.ToList();

            int newspaper = news.Count;
                int num = Cus.Count;
                int Customers = Custom.Count; 
                int salesman = sale.Count;
                ViewBag.cus = Customers;
                ViewBag.message = num;
                ViewBag.sellman = salesman;
            ViewBag.news = newspaper;

            ViewBag.Newspaper = new SelectList(db.Service.ToList(), "NewsPaperName", "NewspaperName");
            //int Cont = db.Customer.Count(t => (t.EndedDate > DateTime.Now));
            //ViewBag.active = Cont;
            //int Coot = db.Customer.Count(t => (t.EndedDate <= DateTime.Now));
            //ViewBag.inactive = Coot;



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


                List<CounterVM> objConter = new List<CounterVM>();


                foreach (var item in cus)
                {
                    CounterVM counter = new CounterVM();
                    counter.NewsPaper = item.NewsPaper;
                    counter.SalesMan = item.SalesMan;
                    counter.Customer = item.Customer;
                    counter.ServiceAssign = item.ServiceAssign;
                    objConter.Add(counter);

                }
                return View(objConter.AsEnumerable());

               
            



            //var Cus = db.ServiceAssign.ToList();
            //    var sale = db.SalesMan.ToList();
            //    int num = Cus.Count;
            //    int salesman = sale.Count;
            //    ViewBag.message = num;
            //    ViewBag.sellman = salesman;
            //    int Cont = db.Customer.Count(t => (t.EndedDate > DateTime.Now));
            //    ViewBag.active = Cont;
            //    int Coot = db.Customer.Count(t => (t.EndedDate <= DateTime.Now));
            //    ViewBag.inactive = Coot;

            //    return View(Cus.ToList());
            //}


            //int BranchId = Convert.ToInt32(Session["BranchId"].ToString());
            //var saleman = db.SalesMan.Where(m=>m.BranchId==BranchId).ToList();
            //var customers = db.Customer.Where(m => m.BranchId == BranchId).ToList();
            //int number = customers.Count;
            //int salesmanNo = saleman.Count;
            //ViewBag.message = number;
            //ViewBag.sellman = salesmanNo;
            ////int Count = db.Customer.Where(m => m.BranchId == BranchId).Count(t => (t.EndedDate > DateTime.Now));       
            ////ViewBag.active = Count;
            ////int cout = db.Customer.Where(m => m.BranchId == BranchId).Count(t => (t.EndedDate <= DateTime.Now));
            ////ViewBag.inactive = cout;
            //return View(db.Customer.Where(m => m.BranchId == BranchId).ToList());

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
                counter.ServiceAssign = item.ServiceAssign;
                var dispatch = item.ServiceAssign.PaperDispatchDate;
                counter.Paperdispatch = ADTOBS.EngToNep(dispatch).ToString();
                var date = item.ServiceAssign.EndedDate;
               counter.NepaliDate = ADTOBS.EngToNep(date).ToString();


                objConter.Add(counter);

            }


            if (objConter.AsEnumerable() == null)
            {
                return HttpNotFound();
            }
            return View(objConter.AsEnumerable());
        }

        //public ActionResult ActiveUser()
        //{
        //    if (Session["Category"].ToString() == "SuperAdmin")
        //    {

        //        var count = db.Customer.Where(t => (t.EndedDate > DateTime.Now));
        //        return View(count.ToList());
        //    }
        //    else
        //    {
        //        int BranchId = Convert.ToInt32(Session["BranchId"].ToString());
        //        var count = db.Customer.Where(m => m.BranchId == BranchId).Where(t => (t.EndedDate > DateTime.Now));
        //        return View(count.ToList());    

        //    }

        //}
        //public ActionResult InActiveUser()
        //{
        //    int BranchId = Convert.ToInt32(Session["BranchId"].ToString());
        //    var count = db.Customer.Where(t => (t.EndedDate <= DateTime.Now));
        //    return View(count.ToList());
        //}


        //public ActionResult Renew(int? id)
        //{
        //    int BranchId = Convert.ToInt32(Session["BranchId"].ToString());

        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Customer customer = db.Customer.Find(id);
        //    if (customer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.SalesManId = new SelectList(db.SalesMan, "Id", "FullName", customer.SalesManId);
        //    ViewBag.ServiceId = new SelectList(db.Service, "Id", "ServiceCode", customer.ServiceId);
        //    return View(customer);
        //}
        //[HttpPost]
        //public ActionResult Renew([Bind(Include = "Id,CustomerId,FirstName,MiddleName,LastName,MPhone,Email,AltEmail,HomeNo,Tole,Address,Provience,Gprslatitude,GprsLongitude,URL,CustomerInfo,CustomerType,TypeDetail,Amount,RegisterDate,RegisteredBy,PaperDispatchDate,Duration,EndedDate,Status,SalesManId,ServiceId")] Customer customer)
        //{
        //    DateTime FinalDate = customer.PaperDispatchDate;
        //    int addedDays = Convert.ToInt32(customer.Duration);
        //    FinalDate = FinalDate.AddDays(addedDays);
        //    var objCustomer = db.Customer.Find(customer.Id);
        //    objCustomer.Id = objCustomer.Id;
        //    objCustomer.CustomerId = objCustomer.CustomerId;
        //    objCustomer.FirstName = objCustomer.FirstName;
        //    objCustomer.MiddleName = objCustomer.MiddleName;
        //    objCustomer.LastName = objCustomer.LastName;
        //    objCustomer.PaperDispatchDate = FinalDate.AddDays(addedDays);
        //    objCustomer.Duration = objCustomer.Duration;

        //    try
        //    {
        //        //db.Customer.Add(customer);
        //        //db.SaveChanges();
        //        String Operation = "Customer ReNew Sucessfully";
        //        string userEmail = Session["userEmail"].ToString();
        //        db.ActivityLog.Add(new ActivityLog
        //        {
        //            Operation = Operation,
        //            CreatedBy = userEmail,
        //            CreatedDate = DateTime.Now

        //        });
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        [HttpGet]
        public ActionResult RecientCostumer()

        {
            var recent = db.ServiceAssign.OrderByDescending(u => u.Id).Take(5);
            return PartialView(recent.ToList());
        }
        //{
        //    try
        //    {
        //        int BranchId = Convert.ToInt32(Session["BranchId"].ToString());
        //        var count = db.Customer.Where(m => m.BranchId == BranchId).OrderByDescending(u => u.Id).Take(5);
        //        return PartialView(count.ToList());
        //    }
        //    catch
        //    {
        //        var Customer = db.Customer.OrderByDescending(u => u.Id).Take(5);
        //        return PartialView(Customer.ToList());
        //    }

            //}
            //[HttpGet]
            //public ActionResult OldestCostumer()
            //{

            //        try
            //        {
            //            int BranchId = Convert.ToInt32(Session["BranchId"].ToString());
            //            var result = db.Customer.Where(x => x.CustomerType == "Normal" && x.BranchId == BranchId).ToList().Select(x => x.Amount).Sum();
            //            ViewBag.message = result;
            //            var count = db.Customer.Where(x => x.CustomerType == "Normal" && x.BranchId == BranchId).Take(5);
            //            return PartialView(count.ToList());
            //        }

            //        catch
            //        {
            //            var result = db.Customer.Where(x => x.CustomerType == "Normal").ToList().Select(x => x.Amount).Sum();
            //            ViewBag.message = result;
            //            var Cus = db.Customer.Where(x => x.CustomerType == "Normal").Take(5);
            //            return PartialView(Cus.ToList());
            //        }


            //}
            //public ActionResult NormalCostumer()
            //{
            //    try
            //    {
            //        int BranchId = Convert.ToInt32(Session["BranchId"].ToString());
            //        var result = db.Customer.Where(x => x.CustomerType == "Normal" && x.BranchId == BranchId).ToList().Select(x => x.Amount).Sum();
            //        ViewBag.message = result;

            //        var count = db.Customer.Where(x => x.CustomerType == "Normal" && x.BranchId == BranchId);
            //        return View(count.ToList());
            //    }
            //    catch {

            //        var result = db.Customer.Where(x => x.CustomerType == "Normal").ToList().Select(x => x.Amount).Sum();
            //        ViewBag.message = result;
            //        var count = db.Customer.Where(x => x.CustomerType == "Normal");
            //        return View(count.ToList());
            //    }

            //}
            //public ActionResult Details(int? id)
            //{
            //    if (id == null)
            //    {
            //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //    }
            //    Customer customer = db.Customer.Find(id);
            //    if (customer == null)
            //    {
            //        return HttpNotFound();
            //    }
            //    return View(customer);
            //}

            //public JsonResult GetCostumers(string term)
            //{

            //    List<string> students = db.Customer.Where(s => s.FirstName.StartsWith(term) || s.MiddleName.StartsWith(term) || s.LastName.StartsWith(term) || (s.FirstName + " " + s.MiddleName).StartsWith(term) || (s.FirstName + " " + s.LastName).StartsWith(term) || (s.FirstName + " " + s.MiddleName + " " + s.LastName).StartsWith(term))
            //.Select(x => x.FirstName + " " + x.MiddleName + " " + x.LastName).ToList();
            //    return Json(students, JsonRequestBehavior.AllowGet);
            //}

            public ActionResult ActivityLog()
        {

            
                var count = db.ActivityLog.OrderByDescending(u => u.Id);
            
                return View(count.ToList());
           
           
        }
        public ActionResult ErrorPage()
        {
            return View("NotFound");
        }
    }
}