using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;  
using System.Web.Mvc;
using Newspaper.Models;
//using PagedList;
using Newspaper.Models.DAL;
using System.Net.Mail;
using System.Data.SqlClient;
using Newspaper.Models.ViewModel;
using Newspaper.Filters;
using System.Configuration;

using System.IO;

namespace Newspaper.Controllers
{
    [SessionCheck(Role ="Supervisor,Admin,SuperAdmin")]
    [ValidateInput(false)]
    public class CustomerController : Controller
    {
        private activities log = new activities();
        private NewspaperEntities db = new NewspaperEntities();

        // GET: /Customer/
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, Customer customer)
        {
            if (Session["Category"].ToString() == "SuperAdmin" || Session["Category"].ToString() =="Counter")
            {
                var Cus = db.Customer.ToList();
                return View(Cus.ToList());

            }

            var Customer = db.Customer.ToList();
            return View(Customer.ToList());

            //int BranchId = Convert.ToInt32(Session["BranchId"].ToString());
            //var customers = db.Customer.Where(m=>m. BranchId==BranchId).ToList();
            //            var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
            //  .Select(x => new { x.Key, x.Value.Errors }).ToArray();
            //return View(customers.ToList());
        }

        public ActionResult AssignedCustomer()
        {
            var assigned = db.ServiceAssign.ToList();
            return View(assigned.ToList());

        }


        // GET: /Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: /Customer/Create
        public ActionResult Create()
        {
            //        if (Session["Category"].ToString() == "SuperAdmin")
            //    {

            //        ViewBag.SalesManId = new SelectList(db.SalesMan, "Id", "FullName");
            //        ViewBag.ServiceId = new SelectList(db.Service, "Id", "NewsPaperName");
            //        ViewBag.BranchId = new SelectList(db.Branch, "BranchId", "BranchName");

            //        return View();
            //    }

            //    int BranchId = Convert.ToInt32(Session["BranchId"].ToString());

            //    ViewBag.SalesManId = new SelectList(db.SalesMan.Where(m => m.BranchId == BranchId).ToList(), "Id", "FullName");
            //    ViewBag.ServiceId = new SelectList(db.Service, "Id", "NewsPaperName");

            //    return View();
            //}
            ViewBag.SalesManId = new SelectList(db.SalesMan, "Id", "FullName");
            ViewBag.ServiceId = new SelectList(db.Service, "Id", "NewsPaperName");
            ViewBag.BranchId = new SelectList(db.Branch, "BranchId", "BranchName");

            return View();
        }



            // POST: /Customer/Create
            // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
            // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            customer.RegisterDate = DateTime.Now;
            customer.RegisteredBy = Session["userEmail"].ToString();


            db.Customer.Add(customer);
            db.SaveChanges();

            try
            {


                
                log.AddActivity("Customer Created Successfully");
                //db.ActivityLog.Add(new ActivityLog
                //{
                //    BranchId = customer.BranchId,
                //    Operation = Operation,
                //    CreatedBy = Session["userEmail"].ToString(),
                //    CreatedDate = DateTime.Now

                //});
                //db.SaveChanges();
                return RedirectToAction("index");
            }
            catch
            {
                return View();
            }
            //if (Session["Category"].ToString() == "SuperAdmin")
            //{
            //    customer.RegisterDate = DateTime.Now;
            //    customer.RegisteredBy = Session["userEmail"].ToString();


            //    db.Customer.Add(customer);
            //    db.SaveChanges();

            //    try
            //    {


            //        String Operation = "Customer Created Sucessfully ";
            //        db.ActivityLog.Add(new ActivityLog
            //        {
            //            BranchId = customer.BranchId,
            //            Operation = Operation,
            //            CreatedBy = Session["userEmail"].ToString(),
            //            CreatedDate = DateTime.Now

            //        });
            //        db.SaveChanges();
            //        return RedirectToAction("index");
            //    }
            //    catch
            //    {
            //        return View();
            //    }
            //}
            //else
            //{
            //    customer.RegisterDate = DateTime.Now;
            //    customer.RegisteredBy = Session["userEmail"].ToString();

            //    db.Customer.Add(customer);
            //    db.SaveChanges();

            //    try
            //    {
            //        int BranchId = Convert.ToInt32(Session["BranchId"].ToString());
            //        String Operation = "Customer Created Sucessfully ";
            //        db.ActivityLog.Add(new ActivityLog
            //        {
            //            BranchId = BranchId,
            //            Operation = Operation,
            //            CreatedBy = Session["userEmail"].ToString(),
            //            CreatedDate = DateTime.Now

            //        });
            //        db.SaveChanges();
            //        return RedirectToAction("index");
            //    }
            //    catch
            //    {
            //        return View();
            //    }
            //}




        }


        // GET: /Customer/Edit/5
        public ActionResult Edit(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchId = new SelectList(db.Branch, "BranchId", "BranchName", customer.BranchId);

             return View(customer);


            //if (Session["Category"].ToString() == "SuperAdmin")
            //{


            //    ViewBag.BranchId = new SelectList(db.Branch, "BranchId", "BranchName",customer.BranchId);

            //    return View(customer);
            //}

            //int BranchId = Convert.ToInt32(Session["BranchId"].ToString());


            ////ViewBag.SalesManId = new SelectList(db.SalesMan, "Id", "FullName", customer.SalesManId);
            ////ViewBag.ServiceId = new SelectList(db.Service, "Id", "NewsPaperName", customer.ServiceId);
            //return View(customer);
        }

        // POST: /Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {

            if (ModelState.IsValid)
            {
                customer.RegisterDate = DateTime.Now;
                customer.RegisteredBy = Session["userEmail"].ToString();
                db.Entry(customer).State = EntityState.Modified;
                try
                {

                    db.SaveChanges();

                    String Operation = "Customer Updated Sucessfully";
                    db.ActivityLog.Add(new ActivityLog
                    {
                        BranchId = customer.BranchId,
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
            return View(customer);
            //if (Session["Category"].ToString() == "SuperAdmin")
            //{
            //    if (ModelState.IsValid)
            //    {
            //        customer.RegisterDate = DateTime.Now;
            //        customer.RegisteredBy = Session["userEmail"].ToString();
            //        db.Entry(customer).State = EntityState.Modified;
            //        try
            //        {

            //            db.SaveChanges();

            //            String Operation = "Customer Updated Sucessfully";
            //            db.ActivityLog.Add(new ActivityLog
            //            {
            //                BranchId = customer.BranchId,
            //                Operation = Operation,
            //                CreatedBy = Session["userEmail"].ToString(),
            //                CreatedDate = DateTime.Now

            //            });
            //            db.SaveChanges();
            //            return RedirectToAction("Index");
            //        }
            //        catch
            //        {
            //            return View();
            //        }
            //    }





            //    return View(customer);
            //}
            //else
            //{
            //    if (ModelState.IsValid)
            //    {
            //        int BranchId = Convert.ToInt32(Session["BranchId"].ToString());
            //        customer.BranchId = BranchId;
            //        customer.RegisterDate = DateTime.Now;
            //        customer.RegisteredBy = Session["userEmail"].ToString();
            //        db.Entry(customer).State = EntityState.Modified;
            //        try
            //        {

            //            db.SaveChanges();

            //            String Operation = "Customer Updated Sucessfully";
            //            db.ActivityLog.Add(new ActivityLog
            //            {
            //                BranchId = BranchId,
            //                Operation = Operation,
            //                CreatedBy = Session["userEmail"].ToString(),
            //                CreatedDate = DateTime.Now

            //            });
            //            db.SaveChanges();
            //            return RedirectToAction("Index");
            //        }
            //        catch
            //        {
            //            return View();
            //        }
            //    }





            //    return View(customer);
            //}
        }
           

        // GET: /Customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: /Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customer.Find(id);
            db.Customer.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //public ActionResult ReNew(int? id)
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ReNew(Customer customer,FormCollection col)
        //{

        //    if (customer.EndedDate >= DateTime.Now)
        //    {
        //        double days = (customer.EndedDate - DateTime.Now).TotalDays;
        //        int day = (int)days;
        //        int addedDays = Convert.ToInt32(customer.Duration);
        //        DateTime endeddate = customer.EndedDate.AddDays(addedDays);

        //        customer.EndedDate = endeddate;
        //    }
        //    else
        //    {
        //        double days = (DateTime.Now - customer.EndedDate).TotalDays;
        //        int day = (int)days;

        //        int addedDays = Convert.ToInt32(customer.Duration);
        //        DateTime endeddate = customer.EndedDate.AddDays(day + addedDays);

        //        customer.EndedDate = endeddate;

        //    }


//            string type = Convert.ToString(col["type"]);
           
//            //DateTime FinalDate = customer.EndedDat e;
//            //int addedDays = Convert.ToInt32(customer.Duration);
//            //FinalDate = FinalDate.AddDays(addedDays);
//            if (type == "paperDispatch")
//            {
//                if (customer.PaperDispatchDate > DateTime.Now)
//                {
//                    var objCust = db.Customer.Find(customer.Id);
//                    double daysDiff = (customer.PaperDispatchDate - objCust.PaperDispatchDate).TotalDays;
//                    customer.EndedDate = customer.EndedDate.AddDays(daysDiff);
//                   // ViewBag.ErrorMessage = "";
//                   // return View();
//;                }
//                DateTime days = customer.PaperDispatchDate;
//                double adddays = (customer.PaperDispatchDate - DateTime.Now).TotalDays;
//                int addeddays = (int)adddays;
//                customer.EndedDate = customer.EndedDate.AddDays(addeddays);


//                //double adddays = (customer.PaperDispatchDate - DateTime.Now).TotalDays;
//                //int addeddays = (int)adddays;
//                //customer.EndedDate = customer.EndedDate.AddDays(addeddays);
//            }
//            else if (type == "renew")
//            {

//                //DateTime days = customer.EndedDate;
//                //int addedDays = Convert.ToInt32(customer.Duration);
//                //DateTime endeddate = customer.EndedDate.AddDays(addedDays);
//                //customer.EndedDate = endeddate;
//                if (customer.EndedDate >= DateTime.Now)
//                {
//                    double days = (customer.EndedDate - DateTime.Now).TotalDays;
//                    int day = (int)days;
//                    int addedDays = Convert.ToInt32(customer.Duration);
//                    DateTime endeddate = customer.EndedDate.AddDays(addedDays);

//                    customer.EndedDate = endeddate;
//                }
//                else {
//                    double days = (DateTime.Now - customer.EndedDate).TotalDays;
//                    int day = (int)days;

//                    int addedDays = Convert.ToInt32(customer.Duration);
//                    DateTime endeddate = customer.EndedDate.AddDays(day + addedDays);

//                    customer.EndedDate = endeddate;

//                }

//            }
//            else if (type == "both")
//            {
//                if (customer.EndedDate >= DateTime.Now)
//                {
//                    if (customer.PaperDispatchDate > DateTime.Now)
//                    {
//                        ViewBag.ErrorMessage = "Your have already Dispached";
//                        return View();
//                    }
//                    DateTime days = customer.PaperDispatchDate;
//                    double adddays = (customer.PaperDispatchDate - DateTime.Now).TotalDays;
//                    int addeddays = (int)adddays;
//                    int totalRenewDays = Convert.ToInt32(customer.Duration) + addeddays;
//                    customer.EndedDate = customer.EndedDate.AddDays(totalRenewDays);
//                }
//                else
//                {

//                    if (customer.PaperDispatchDate > DateTime.Now)
//                    {
//                        ViewBag.ErrorMessage = "Your have already Dispached";
//                        return View();
//                    }
//                    double Days = (DateTime.Now - customer.EndedDate).TotalDays;
//                    int day = (int)Days;
//                    DateTime days = customer.PaperDispatchDate;
//                    double adddays = (customer.PaperDispatchDate - DateTime.Now).TotalDays;
//                    int addeddays = (int)adddays;
//                    int totalRenewDays = Convert.ToInt32(customer.Duration) + addeddays;
//                    customer.EndedDate = customer.EndedDate.AddDays(totalRenewDays + day);
//                }
//            }

            


        //    var objCustomer = db.Customer.Find(customer.Id);
        //    objCustomer.Id = objCustomer.Id;
        //    objCustomer.CustomerId = objCustomer.CustomerId;
        //    objCustomer.FirstName = objCustomer.FirstName;
        //    objCustomer.MiddleName = objCustomer.MiddleName;
        //    objCustomer.LastName = objCustomer.LastName;
        //    objCustomer.EndedDate = customer.EndedDate;
        //    objCustomer.Duration = objCustomer.Duration;
        //    objCustomer.PaperDispatchDate =customer.PaperDispatchDate;
        //    objCustomer.NepaliDate = customer.NepaliDate;


        //    try
        //    {
        //        //db.Customer.Add(customer);
        //        db.SaveChanges();
        //        int BranchId = Convert.ToInt32(Session["BranchId"].ToString());
        //        String Operation = "Customer ReNew Sucessfully";
        //        string userEmail = Session["userEmail"].ToString();
        //        db.ActivityLog.Add(new ActivityLog
        //        {
        //            BranchId = BranchId,
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        [HttpPost]
        public JsonResult getEmployee(int id)
        {
            Customer objemp = db.Customer.FirstOrDefault(m => m.Id == id);
            return Json(objemp);
        }

        //public void sendMail()
        //{
        //    List<string> email = db.Customer.Where(s => (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) <= 10))
        //   .Select(t => t.Email).ToList();


        //    string from = "dbugtest2016@gmail.com";
        //    string fromPassword = "my@test#";

        //    if (email != null)
        //    {
        //        int i;
        //        for (i = 0; i <= email.Count; i++)
        //        {
        //            try
        //            {
        //                string str = "<font size='5'>Regards <br/> <b>DebugSoft</b></font>";
        //                using (MailMessage mail = new MailMessage(from, email[i]))
        //                {
        //                    List<DateTime> days = db.Customer.Where(s => (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) <= 10))
        //  .Select(t => t.EndedDate).ToList();

        //                    List<string> Name = db.Customer.Where(s => (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) <= 10))
        //  .Select(t => t.FirstName).ToList();

        //                    mail.Subject = "Newspaper Date is being expired";
        //                    //mail.Body = "Dear"+Name[i]+","
        //                    //    +"Your Remaining NewsPaper Days is"+(days[i]-DateTime.Now).Days+"Days";

        //                    //mail.Body = "<html><body><p>Dear " + Name[i] + ",</p><p>Your Remaining NewsPaper Days is " + (days[i] - DateTime.Now).Days + " Days.</p><p>Regards,<br>-DE!BUGSOFT</br></p></body></html>";
        //                    if ((days[i] - DateTime.Now).Days > 0)
        //                    {
        //                        mail.Body = "Dear" + " " + Name[i] + "<Br/> Your expiring date is near,Please renew in " + " " + (days[i] - DateTime.Now).Days + " " + "days for uninterupted subscription<br/><br/>" + str;
        //                    }
        //                    else if ((days[i] - DateTime.Now).Days == 0)
        //                    {
        //                        mail.Body = "Dear" + " " + Name[i] + "<Br/> Your Account is expired today.Please Contact our office for reactivation.<br/><br/>" + str;
        //                    }
        //                    else
        //                    {
        //                        mail.Body = "Dear" + " " + Name[i] + "<Br/> Your Account was expired" + " " + (DateTime.Now - days[i]).Days + " " + " days ago .Please Contact our office for reactivation.<br/><br/>" + str;
        //                    }

        //                    mail.IsBodyHtml = true;
        //                    SmtpClient smtp = new SmtpClient();
        //                    smtp.Host = "smtp.gmail.com";
        //                    smtp.EnableSsl = true;
        //                    smtp.UseDefaultCredentials = false;
        //                    NetworkCredential networkCredential = new NetworkCredential(from, fromPassword);

        //                    smtp.Credentials = networkCredential;
        //                    smtp.Port = 587;
        //                    smtp.Send(mail);
        //                    ViewBag.Message = "Your Password Is Sent to your email";
        //                }
        //            }
        //            catch
        //            {
        //                ViewBag.Message = "Email sending failed";
        //            }

        //        }

        //    }
        //}



    }
}

