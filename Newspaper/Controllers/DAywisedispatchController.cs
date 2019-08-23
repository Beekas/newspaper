using Newspaper.Models;
using Newspaper.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Newspaper.Controllers
{
    public class DAywisedispatchController : Controller
    {
        private NewspaperEntities db;
        // GET: DAywisedispatch
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult create()
        {
            List<int> serviceCount = new List<int>(10);
            int salesManId = 2;
            DaywisePaperDispatch paperDispatchCount = new DaywisePaperDispatch();
            var salesmanServices = db.day.Where(m => m.SalesManId == salesManId && m.PaperDispatchDate <= DateTime.Now && m.EndedDate >= DateTime.Now);
            var services = from m in salesmanServices
                           group m by new { m.ServiceId } into mygroup
                           select mygroup.FirstOrDefault();
            int i = 0;
            foreach (var item in services.ToList())
            {
                int dispatchCount = salesmanServices.Where(m => m.ServiceId == item.ServiceId).Count();
                serviceCount[i] = dispatchCount;
                i++;
            }
            return View();
        }
        public JsonResult getPaperDispatchCount(int salesManId)
        {
            List<int> serviceCount = new List<int>(10);
            DaywisePaperDispatch paperDispatchCount = new DaywisePaperDispatch();
            var salesmanServices = db.Customer.Where(m => m.SalesManId == salesManId && m.PaperDispatchDate < DateTime.Now && m.EndedDate > DateTime.Now);
            var services = from m in salesmanServices
                           group m by new { m.ServiceId } into mygroup
                           select mygroup.FirstOrDefault();
            int i = 0;
            foreach (var item in services.ToList())
            {
                int dispatchCount = salesmanServices.Where(m => m.ServiceId == item.ServiceId).Count();
                serviceCount[i] = dispatchCount;
                i++;
            }

            return Json(serviceCount);
        }
        //public int CountSalesMan { get; set; }

        //        var images = db.Day.GroupBy(n => n.CategoryName)
        //                            .Select(g => new { CategoryName = g.Key, Count = g.Count() }).ToList();
        //        List<DisplayImage> imagesList = new List<DisplayImage>();
        //    for (int i = 0; i<images.ToList().Count; i++)
        //    {
        //        imagesList.Add(new DisplayImage { Categoryname = images[i].CategoryName, Count = images[i].Count
        //    });

        //    return View(imagesList);

    }
}