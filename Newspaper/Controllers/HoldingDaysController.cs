using Newspaper.Models;
using Newspaper.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Newspaper.Controllers
{
    public class HoldingDaysController : Controller
    {
        private NewspaperEntities db = new NewspaperEntities();
        // GET: HoldingDays
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(int? id)
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
            ViewBag.SalesManId = new SelectList(db.SalesMan, "Id", "FullName", customer.SalesManId);
            ViewBag.ServiceId = new SelectList(db.Service, "Id", "NewsPaperName", customer.ServiceId);

            return View(customer);
        }

        [HttpPost]
        public ActionResult Create(Customer customer,FormCollection col)
        {
           
            int days =Convert.ToInt32(col["HoldingDays"].ToString());
            string datestr = col["EndedDate"];
            DateTime date = Convert.ToDateTime(col["EndedDate"]);
            var objCustomer = db.Customer.Find(customer.Id);
           // int remainDays = objCustomer.EndedDate-DateTime.Now;
            DateTime holdingEndedDate = date.AddDays(days);
          //  DateTime customerEndedDate = date.AddDays(days+|)
            if (date > DateTime.Now)
            { 

                customer.PaperDispatchDate = customer.EndedDate.AddDays(days);

            }
            db.HoldingDays.Add(new HoldingDays { CustomerId = customer.Id, HoldingStartDays = date, HoldingEndDays = customer.EndedDate });
          db.SaveChanges();

           
            objCustomer.EndedDate = customer.EndedDate;
            db.SaveChanges();
            return View();
        }
        public ActionResult Resume()
        {
            return View();
        }
    }
}