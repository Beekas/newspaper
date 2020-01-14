using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Newspaper.Models;
using System.Web.Security;
using System.Web.UI.WebControls;
using Newspaper.Filters;
using Newspaper.Models.DAL;
using RegistrationAndLogin;
using System.Net.Mail;
using System.Net;
using System.IO;
using Newspaper.Models.ViewModel;
using System.Net.Configuration;
using System.Configuration;

namespace Newspaper.Controllers
{
    // [Authorize]

    public class AccountController : Controller
    {
        private activities log = new activities();

        NewspaperEntities _db = new NewspaperEntities();
        [SessionCheck]
        [ValidateInput(false)]
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Login(Models.LoginViewModel l, string ReturnUrl = "")
        {
            using (NewspaperEntities db = new NewspaperEntities())
            {

                //var Admin = _db.Admin.FirstOrDefault(a => (a.UserName == l.UserName || a.Email == l.UserName));
                //if (Admin != null)
                //{
                //    var pass = Crypto.Hash(l.Password);
                //    if (string.Compare(pass, Admin.Password) == 0)
                //    {
                //        FormsAuthentication.SetAuthCookie(l.UserName, l.RememberMe);
                //        if (Url.IsLocalUrl(ReturnUrl))
                //        {
                //            var objAdmin = _db.Admin.FirstOrDefault(a=>(a.UserName == l.UserName || a.Email == l.UserName) && a.Password == l.Password);


                //            return RedirectToAction("Index","Dashboard");
                //        }
                //        else
                //        {
                //            Session.Add("id", Admin.Id);
                //            Session.Add("userName", Admin.UserName);
                //            Session.Add("userEmail", Admin.Email);
                //            Session.Add("FullName", Admin.FullName);
                //            Session.Add("Category", Admin.Category);
                //            return RedirectToAction("Index", "DashBoard");
                //        }
                //    }
                //}
                //else
                //{
                //    ModelState.AddModelError("", "Invalid User");
                //}
                //    return View();
                //}


                var Admin = _db.Admin.FirstOrDefault(a => (a.UserName == l.UserName || a.Email == l.UserName));
                if (Admin != null)
                {
                    var pass = Crypto.Hash(l.Password);
                    if (string.Compare(pass, Admin.Password) == 0)
                    {
                        var check = _db.Admin.FirstOrDefault(a => a.Status);
                        if (Admin.Status == true)
                        {
                            string imageUrl = "../../img/17.jpg";
                            if (System.IO.File.Exists(Server.MapPath(@"~/images/" + Admin.Id + ".jpg")))
                            {
                                imageUrl = "../../images/" + Admin.Id + ".jpg";
                            }
                            Session.Add("id", Admin.Id);
                            Session.Add("userName", Admin.UserName);
                            Session.Add("userEmail", Admin.Email);
                            Session.Add("FullName", Admin.FullName);
                            Session.Add("Category", Admin.Category);
                            Session.Add("imageUrl", imageUrl);
                            Session.Add("BranchId", Admin.BranchId);


                            if (Session["Category"].ToString() == "Counter")
                            {
                                var login = Session["userEmail"].ToString();
                                log.AddActivity(login +" "+ "logged in successfully");
                                return RedirectToAction("Index", "Counter");
                            }
                            else if (Session["Category"].ToString() == "Accountant")
                            {
                                var login = Session["userEmail"].ToString();
                                log.AddActivity(login + " " + "logged in successfully");
                                return RedirectToAction("Index", "Accountant");
                            }
                            else
                            {

                                return RedirectToAction("Index", "DashBoard");
                            }


                        }
                        else
                        {
                            ModelState.AddModelError("", "Admin is Deactivate");

                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "UserName and Password not match");

                    }
                }
                else
                {
                    ModelState.AddModelError("", "UserName and Password not match");

                }
            }
            return View();

        }
        public ActionResult AdminDetial()
        {
            int id = Convert.ToInt32(Session["id"].ToString());
            Admin admin = _db.Admin.Find(id);
            return View(admin);
        }
        [Authorize]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword(Admin admin)
        {


            //string from = "usern";
            //string fromPassword = "my@test#";

            string password = Membership.GeneratePassword(6, 1);

            if (admin.Email != null)
            {
                var mail = new MailMessage();

                //using (MailMessage mail = new MailMessage(admin.Email))


                try
                {
                    var tb = _db.Admin.Where(u => u.Email == admin.Email).FirstOrDefault();

                    if (tb == null)
                    {
                        ViewBag.Message = "email Doesnot Exist Please enter valid email";
                    }
                    else
                    {
                        mail.To.Add(new MailAddress(admin.Email));

                        mail.Subject = "Password Recovery";
                        mail.Body = "Use this Password to login:" + password;


                        mail.IsBodyHtml = false;


                        using (var smtp = new SmtpClient())
                            //smtp.Host = "smtp.gmail.com";
                            //smtp.EnableSsl = true;
                            //smtp.UseDefaultCredentials = false;
                            //NetworkCredential networkCredential = new NetworkCredential(from, fromPassword);

                            //smtp.Credentials = networkCredential;
                            //smtp.Port = 587;
                            smtp.Send(mail);

                        ViewBag.Message = "Your Password Is Sent to your email";
                        var query = (from q in _db.Admin
                                     where q.Email == admin.Email
                                     select q).First();
                        query.randompass = password;
                        _db.SaveChanges();

                    }
                }
                catch (Exception e)
                {
                    return RedirectToAction(e.ToString().Substring(0,100)+"errorpage");
                }
                TempData["Message"] = admin.Email;
                TempData.Keep();
                return RedirectToAction("conformation");
            }
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
             .Select(x => new { x.Key, x.Value.Errors }).ToArray();
            return View();


            //return RedirectToAction("Index", "Home");
        }
        public ActionResult conformation()
        {
            if (TempData["Message"] != null)
            {
                string message = TempData["Message"].ToString();
                TempData.Keep();
                if (message != null)
                {
                    return PartialView();
                }
            }
            return RedirectToAction("ForgetPassword");

        }
        [HttpPost]
        public ActionResult conformation(Admin admin)
        {

            string message = TempData["Message"].ToString();
            TempData.Keep();
            TempData["info"] = (_db.Admin.Any(u => u.Email == message && u.randompass == admin.randompass));
            if (admin.randompass != null)
            {
                if (_db.Admin.Any(u => u.Email == message && u.randompass == admin.randompass))
                {
                    return RedirectToAction("NewPassword");
                }
            }
            //  return PartialView();


            ViewBag.message = "conformation code donot match";
            return PartialView();
        }
        public ActionResult NewPassword()
        {

            if (TempData["info"] != null && TempData["Message"] != null)
            {
                TempData.Keep();
                return PartialView();

            }
            return RedirectToAction("ForgetPassword");
        }
        [HttpPost]
        public ActionResult NewPassword(PasswordConform pass)
        {
            if (ModelState.IsValid)
            {

                string message = TempData["Message"].ToString();
                var query = (from q in _db.Admin
                             where q.Email == message
                             select q).First();
                string password = Crypto.Hash(pass.Password);
                query.Password = password;
                query.randompass = null;
                _db.SaveChanges();
                return RedirectToAction("Login");



            }
            return PartialView();
        }
        public ActionResult errorpage()
        {
            return PartialView();
        }

    }

}
