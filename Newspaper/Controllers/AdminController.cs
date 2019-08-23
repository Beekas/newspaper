using Newspaper.Filters;
using Newspaper.Models;
using Newspaper.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using Newspaper.Models.DAL;
using System.IO;
using RegistrationAndLogin;
using System.Net;
using System.Data.Entity;

namespace Newspaper.Controllers
{
    [SessionCheck(Role ="SuperAdmin,Admin")]
    [ValidateInput(false)]
    public class AdminController : Controller
    {

        public ActionResult Index()
        {
            if (Session["Category"].ToString() == "SuperAdmin"|| Session["Category"].ToString() == "Supervisor")
            {
                List<AdminViewModel> lstAdmin = new List<AdminViewModel>();
                var Admin = _db.Admin.ToList();
                foreach (var item in Admin)
                {
                    lstAdmin.Add(new AdminViewModel() { Id = item.Id, EmployeeId = item.EmployeeId, FullName = item.FullName, Email = item.Email, UserName = item.UserName, Password = item.Password, Phone = item.Phone, WorkPhone = item.WorkPhone, URL = item.URL, PPSizePhoto = item.PPSizePhoto, CreatedDate = item.CreatedDate, ModifiedDate = item.ModifiedDate, CreatedBy = item.CreatedBy, ModifiedBy = item.ModifiedBy });

                }

                return View(lstAdmin as IEnumerable<AdminViewModel>);
            }
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);



        }

        NewspaperEntities _db = new NewspaperEntities();

        [SessionCheck(Role = "SuperAdmin,Admin,Supervisor,Accountant,Counter")]
        public ActionResult ChangePassword()
        {
            if (Session["id"]!=null)
            {
                {
                    return View();
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }
        [HttpPost]
        [SessionCheck(Role = "SuperAdmin,Admin,Supervisor,Accountant,Counter")]
        public ActionResult ChangePassword(ChangePasswordViewModel ch)
        {
            int employeeid = Convert.ToInt32(Session["Id"].ToString());

            Admin us = _db.Admin.Single(m => m.Id == employeeid);// (u => u.Id == employeeid && u.Password == ch.OldPassword).FirstOrDefault();
            if (us != null)
            {
                var pass = Crypto.Hash(ch.OldPassword);
                if (string.Compare(pass, us.Password) == 0)
                   
                {
                    if (ch.NewPassword == ch.ConfirmNew)
                    {
                        us.Password = Crypto.Hash(ch.NewPassword);
                        _db.SaveChanges();
                        ViewBag.Message = "Password Changed Successfully";
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "New Password and Confirm Password Mismatched";
                    }
                }
                else {
                    ViewBag.ErrorMessage = "Password not matched with previous one!";
                }
            }
            ModelState.Clear();
            return View();
        }

        public ActionResult CreateAdmin()
        {
            ViewBag.BranchId = new SelectList(_db.Branch, "BranchId", "BranchName");
            if (Session["Category"].ToString() == "SuperAdmin")
            {
                {
                    return View();
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }
        public ActionResult uploadFile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult uploadFile(HttpPostedFileBase file)
        {
            string name = file.FileName;
            return View();
        }
        [HttpPost]
        public ActionResult CreateAdmin(Admin tb, HttpPostedFileBase ImageFile)
        {
            Admin admin = _db.Admin.FirstOrDefault(m => m.Email == tb.Email);
            if (admin == null)
            {
               
                    tb.ModifiedDate = DateTime.Now;
                    tb.ModifiedBy = Session["userEmail"].ToString();
                    tb.CreatedDate = DateTime.Now;
                    tb.CreatedBy = Session["userEmail"].ToString();
                    tb.Password = Crypto.Hash(tb.Password);
                    var objAdmin = _db.Admin.OrderByDescending(m => m.Id).FirstOrDefault();


                    HttpFileCollectionBase image = Request.Files;
                    
                   // string fileName1 = ImageFile.FileName;
                  
                   string filename = (objAdmin.Id+1).ToString() + ".jpg";

                    string name = "/Images/";
                    bool exist = System.IO.Directory.Exists(Server.MapPath(name));
                    if (!exist)
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(name));
                    }

                   // string filename = Path.GetFileNameWithoutExtension(tb.ImageFile.FileName);
                   // string extension = Path.GetExtension(tb.ImageFile.FileName);


                    //filename = filename + extension;



                    tb.PPSizePhoto = "/images/" + filename;

                if (ImageFile != null)
                {
                    filename = Path.Combine(Server.MapPath("~/Images/"), filename);
                    tb.ImageFile.SaveAs(filename);
                }

                    try
                    {
                        _db.Admin.Add(tb);
                        _db.SaveChanges();
                        String Operation = "Admin Created Sucessfully";
                        _db.ActivityLog.Add(new ActivityLog
                        {
                            Operation = Operation,
                            CreatedBy = Session["userEmail"].ToString(),
                            CreatedDate = DateTime.Now

                        });
                        _db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        return View();
                    }
                
            }
            else
            {
                ViewBag.ErrorMessage = "Email Alredy Exist";
            }
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (Session["Category"].ToString() == "SuperAdmin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Admin tb = new Admin();
                tb = _db.Admin.Where(x => x.Id == id).FirstOrDefault();

                Admin Admin = _db.Admin.Find(id);
                if (Admin == null)
                {
                    return HttpNotFound();
                }
                return View(Admin);
            }
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }
        public ActionResult Edit(int? id)
        {
            if (Session["Category"].ToString() == "SuperAdmin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Admin admin = _db.Admin.Find(id);
                if (admin == null)
                {
                    return HttpNotFound();
                }

                return View(admin);
            }
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        // POST: /Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Admin admin, HttpPostedFileBase ImageFile)
        {

            if (ModelState.IsValid)
            {
                var objadmin = _db.Admin.Find(admin.Id);
                objadmin.CreatedDate = DateTime.Now;
                objadmin.CreatedBy = Session["userEmail"].ToString();
                objadmin.ModifiedDate = DateTime.Now;
                objadmin.ModifiedBy = Session["userEmail"].ToString();
                objadmin.EmployeeId = admin.EmployeeId;
                objadmin.FullName = admin.FullName;
                objadmin.Phone = admin.Phone;
                objadmin.URL = admin.URL;
                objadmin.Email = admin.Email;
                objadmin.WorkPhone = admin.WorkPhone;

                objadmin.Status = admin.Status;

                objadmin.UserName = admin.UserName;
                objadmin.Category = admin.Category;




                HttpFileCollectionBase image = Request.Files;
                if (ImageFile != null)
                {

                    string fileName1 = admin.Id + ".jpg";
                    ///string filename = Path.GetFileNameWithoutExtension(admin.ImageFile.FileName);
                    //string extension = Path.GetExtension(admin.ImageFile.FileName);
                    //filename = admin.Id + extension;
                    objadmin.PPSizePhoto = "/images/" + fileName1;
                    string filename = Path.Combine(Server.MapPath("~/Images/"), fileName1);
                    ImageFile.SaveAs(filename);
                }



                try
                {

                    _db.SaveChanges();
                    String Operation = "Admin Updated Sucessfully";
                    _db.ActivityLog.Add(new ActivityLog
                    {
                        Operation = Operation,
                        CreatedBy = Session["userEmail"].ToString(),
                        CreatedDate = DateTime.Now

                    });
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
             .Select(x => new { x.Key, x.Value.Errors }).ToArray();
            return View(admin);


        }

        // GET: /Customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin Admin = _db.Admin.Find(id);
            try
            {
                _db.Admin.Remove(Admin);
                _db.SaveChanges();
                String Operation = "Admin Deleted Sucessfully";
                _db.ActivityLog.Add(new ActivityLog
                {
                    Operation = Operation,
                    CreatedBy = Session["userEmail"].ToString(),
                    CreatedDate = DateTime.Now

                });
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }




    }
}