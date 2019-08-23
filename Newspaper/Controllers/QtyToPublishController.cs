using Newspaper.Filters;
using Newspaper.Models;
using Newspaper.Models.DAL;
using Newspaper.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Newspaper.Controllers
{
    [ValidateInput(false)]
    [SessionCheck(Role = "SuperAdmin,Supervisor")]
    public class QtyToPublishController : Controller
    {
        private NewspaperEntities db = new NewspaperEntities();
        // GET: QtyToPublish
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SelectPublishDate()
        {


            return View();
        }
        public ActionResult NewspaperToPrint(List<SelectSalesManVM> publishNewspaper)
        {


            return View();
        }
        [HttpPost]
        public ActionResult NewspaperToPrint(FormCollection col)
        {


            List<SelectSalesManVM> publishNewspaper = new List<SelectSalesManVM>();
            
            DateTime date = Convert.ToDateTime(col["NewpaperToPublish"]);
            string Date = col["NepDate"].ToString();
            if (date > DateTime.Now)
            {


                NewspaperEntities db = new NewspaperEntities();


                string Email = Session["userEmail"].ToString();

                //DateTime endedDate = db.Customer.Include("Service").FirstOrDefault(m => m.EndedDate > date).EndedDate;
                var Newspaper = db.ServiceAssign.FirstOrDefault(m => m.EndedDate > date && m.PaperDispatchDate < date);


                var Paper = db.ServiceAssign.Where(m => m.EndedDate > date && m.PaperDispatchDate < date).GroupBy(n => n.NewspaperId)
                                        .Select(g => new { ServiceId = g.Key, Count = g.Sum(m => m.Quantity ),newspaperId=g.FirstOrDefault().NewspaperId }).ToList();

                if (Newspaper == null)
                {
                    TempData["message"] = "No record found.";
                    return RedirectToAction("SelectPublishDate");
                }
                for (int i = 0; i < Paper.ToList().Count; i++)
                {
                    int newsId = Int32.Parse(Paper[i].newspaperId.ToString());
                    string newspapername = db.Service.FirstOrDefault(m => m.Id == newsId).NewsPaperName;
                    publishNewspaper.Add(new SelectSalesManVM { NepDate = Date, NewsPaperName =newspapername, Count = Paper[i].Count, branch = "All Branches" });
                    // imagesList.Add(new SelectSalesManVM { NewsPaperName=customer[i].ServiceId, Count = customer[i].Count });
                }

                try
                {
                    string operation = "Estimated Report to print Newspaper is created by" + " " + Email;
                    db.ActivityLog.Add(new ActivityLog
                    {
                        Operation = operation,
                        CreatedBy = Session["userEmail"].ToString(),
                        CreatedDate = DateTime.Now
                    });
                    db.SaveChanges();
                }
                catch
                {
                    ViewBag.ErrorMessage = "Estimated Report Failed To Print";
                }
                return View(publishNewspaper);



            }
            else
            {
                TempData["message"] = "You cannot Estimate for this date";
                return RedirectToAction("SelectPublishDate");
            }
        }


    

        //public List<SelectSalesManVM> qtyprint(DateTime date, string Date, int BranchId, int branch)
        //{
            
        //    List<SelectSalesManVM> publishNewspaper = new List<SelectSalesManVM>();


        //    NewspaperEntities db = new NewspaperEntities();
        //    var Newspaper = db.Customer.Include("Service").FirstOrDefault(m => m.EndedDate > date && m.PaperDispatchDate < date && m.BranchId == BranchId);


        //    var Paper = db.Customer.Where(m => m.EndedDate > date && m.PaperDispatchDate < date &&  m.BranchId == BranchId).GroupBy(n => n.Service.NewsPaperName)
        //                            .Select(g => new { ServiceId = g.Key, Count = g.Sum(m => m.Qunatity) }).ToList();

        //    if (Newspaper == null)
        //    {
        //        TempData["message"] = "No record found.";
        //        return null;
        //    }
        //    var ObjBranch = db.Branch.Find(BranchId);
        //    for (int i = 0; i < Paper.ToList().Count; i++)
        //    {
        //        publishNewspaper.Add(new SelectSalesManVM
        //        {
        //            NepDate = Date,
        //            NewsPaperName = Paper[i].ServiceId,
        //            Count = Paper[i].Count,
        //            branch = ObjBranch.BranchName,
        //        });
        //        // imagesList.Add(new SelectSalesManVM { NewsPaperName=customer[i].ServiceId, Count = customer[i].Count });
        //    }
        //    try
        //    {
        //        string Email = Session["userEmail"].ToString();
        //        String operation = "Estimated to Print Newspaper is created by" + " " + Email ;
        //        db.ActivityLog.Add(new ActivityLog
        //        {
        //            BranchId = branch,
        //            Operation = operation,
        //            CreatedBy = Session["userEmail"].ToString(),
        //            CreatedDate = DateTime.Now
        //        });
        //        db.SaveChanges();
        //    }
        //    catch
        //    {
        //        ViewBag.ErrorMessage = "Estimated Report print failed";
        //    }
        //    return publishNewspaper;
        //}

    }

}

