using Newspaper.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models
{
   
    public class activities
    {
        private NewspaperEntities db = new NewspaperEntities();
        public void AddActivity(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                db.ActivityLog.Add(new ActivityLog
                {
                    //BranchId = customer.BranchId,
                    Operation = message,
                    CreatedBy =HttpContext.Current.Session["userEmail"].ToString(),
                    CreatedDate = DateTime.Now

                });
                db.SaveChanges();
            }
        }
    }
}