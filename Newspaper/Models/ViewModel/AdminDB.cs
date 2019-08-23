using Newspaper.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models.ViewModel
{
    public class AdminDB
    {
        NewspaperEntities _db = new NewspaperEntities();
        public int CreateAdmin(AdminViewModel av)
        {
            Admin tb = new Admin();
            tb.EmployeeId = av.EmployeeId;
            tb.FullName = av.FullName;
            tb.Email = av.Email;
            tb.UserName = av.UserName;
            tb.Password = av.Password;
            tb.Phone = av.Phone;
            tb.WorkPhone = av.WorkPhone;
            tb.URL = av.URL;
            tb.PPSizePhoto = av.PPSizePhoto;
            tb.CreatedDate = DateTime.Now;// av.CreatedDate;
            tb.ModifiedDate = DateTime.Now; //av.ModifiedDate;
            tb.CreatedBy = av.CreatedBy;
            tb.ModifiedBy = av.ModifiedBy;
            _db.Admin.Add(tb);
            return _db.SaveChanges();
        }
    }
}