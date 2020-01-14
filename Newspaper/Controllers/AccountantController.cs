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
    [SessionCheck(Role ="Accountant")]
    [ValidateInput(false)]
    public class AccountantController : Controller
    {
        
        private NewspaperEntities db = new NewspaperEntities();

        // GET: Counter
        public ActionResult Index()
        {
            var cus = (from s in db.ServiceAssign
                       from c in db.Customer
                       from n in db.Service
                       where s.NewspaperId == n.Id && s.CustomerId == c.Id
                       select new
                       {
                           Customer = c,
                           ServiceAssign = s,
                           NewsPaper = n
                       }).ToList(); // as IEnumerable<CounterVM>;
            var uniqCustomer = from m in cus
                               group m by new { m.Customer.Id }
                               into mygroup
                               select mygroup.FirstOrDefault();


            List<AccountantVM> objConter = new List<AccountantVM>();


            foreach (var item in cus)
            {
                AccountantVM counter = new AccountantVM();
                counter.Service = item.NewsPaper;
                counter.Customer = item.Customer;
                var end = item.ServiceAssign.EndedDate;
                counter.Endeddate = ADTOBS.EngToNep(end).ToString();
                counter.ServiceAssign = item.ServiceAssign;
               
                objConter.Add(counter);

            }
            return View(objConter.AsEnumerable());

        }

        public ActionResult ViewAccount(int? id)
        {
            var cus = (from a in db.Account
                       from c in db.Customer
                       from n in db.Service
                       from s in db.ServiceAssign
                       from d in db.SalesMan
                       where a.NewspaperId == n.Id && a.CustomerId==id.Value && a.CustomerId == c.Id && (a.CustomerId == s.CustomerId && a.NewspaperId==s.NewspaperId) && s.SalesManId==d.Id
                       select new
                       {
                           Customer = c,
                           ServiceAssign = s,
                           Account = a,
                           SalesMan = d,
                           NewsPaper = n
                       }).ToList(); // as IEnumerable<CounterVM>;
            if (cus.Count == 0)
            {
                TempData["ErrorMessage"] = "No Record Found.";
                return RedirectToAction("index");
            }
            List<AccountantVM> objConter = new List<AccountantVM>();
            foreach (var item in cus)
            {
                AccountantVM counter = new AccountantVM();
                counter.Service = item.NewsPaper;
                counter.Customer = item.Customer;
                counter.ServiceAssign = item.ServiceAssign;
                counter.SalesMan = item.SalesMan;
                counter.Account = item.Account;

                objConter.Add(counter);

            }
            return View(objConter.AsEnumerable());

        }


        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Account account = db.Account.Find(id);
        //    if (account == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(account);
        //}

        //// GET: Accountant/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Accountant/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Payoption,BankName,BankAcc,Amount,Paymentdate,Nepalidate,CreatedDate,UpdateDate,CreatedBy,UpdatedBy,CustomerId,NewspaperId,BillNo")] Account account)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Account.Add(account);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(account);
        //}

        //// GET: Accountant/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Account account = db.Account.Find(id);
        //    if (account == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(account);
        //}

        //// POST: Accountant/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Payoption,BankName,BankAcc,Amount,Paymentdate,Nepalidate,CreatedDate,UpdateDate,CreatedBy,UpdatedBy,CustomerId,NewspaperId,BillNo")] Account account)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(account).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(account);
        //}

        //// GET: Accountant/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Account account = db.Account.Find(id);
        //    if (account == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(account);
        //}

        //// POST: Accountant/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Account account = db.Account.Find(id);
        //    db.Account.Remove(account);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
