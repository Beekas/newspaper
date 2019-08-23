using Newspaper.Filters;
using Newspaper.Models;
using Newspaper.Models.DAL;
using Newspaper.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Newspaper.Controllers
{
    [SessionCheck]
    public class MailController : Controller
    {
        private NewspaperEntities db = new NewspaperEntities();


        // GET: Mail
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendMail()
        {
            return View();
        }


        [HttpGet]
        public ActionResult CustomMail()
        {
            List<string> email = db.Customer.Select(t => t.Email).ToList();
            ViewBag.Email = email;
            return View();
        }


        [HttpPost]
        public ActionResult CustomMail(MailViewModel mm)
        {

            List<string> email = db.Customer.Where(s => (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) >= 0))
           .Select(t => t.Email).ToList();
            //string from = "dbugtest2016@gmail.com";
            // string fromPassword = "my@test#";

            if (email != null)
            {
                try
                {
                    int i;
                    for (i = 0; i <= email.Count; i++)
                    {
                        try
                        {
                            string str = "<font size='5'>Regards <br/> <b>DebugSoft</b></font>";
                            using (MailMessage mail = new MailMessage(/*from, email[i]*/))
                            {
                                List<string> Name = db.Customer.Where(s => (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) >= 0))
              .Select(t => t.FirstName).ToList();

                                mail.To.Add(new MailAddress(email[i]));
                                mail.Subject = mm.MailSubject;

                                mail.IsBodyHtml = true;
                                mail.Body = "Dear" + " " + Name[i] + "," + mm.MailBody + "<br/>" + "Thank You<br/>" + str;


                                SmtpClient smtp = new SmtpClient();
                                //smtp.Host = "smtp.gmail.com";
                                //smtp.EnableSsl = true;
                                //smtp.UseDefaultCredentials = false;
                                //NetworkCredential networkCredential = new NetworkCredential(from, fromPassword);

                                //smtp.Credentials = networkCredential;
                                //smtp.Port = 587;
                                smtp.Send(mail);
                            }
                        }
                        catch
                        {
                            ViewBag.Message = "Custom Mail sent";
                        }
                    }
                }
                catch
                {
                    ViewBag.ErrorMessage = "Email sending failed";
                }
                try
                {
                    String Operation = "Custom mail sent";
                    db.ActivityLog.Add(new ActivityLog
                    {
                        Operation = Operation,
                        CreatedBy = Session["userEmail"].ToString(),
                        CreatedDate = DateTime.Now

                    });
                    db.SaveChanges();
                }
                catch
                {
                    ViewBag.ErrorMessage = "Email sending failed";
                }

            }


            return View();
        }

        public ActionResult ExpiredMail()
        {
            if (Session["Category"].ToString() == "SuperAdmin")
            {
                List<string> email = db.Customer.Where(s => (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) < 0))
               .Select(t => t.Email).ToList();


                //string from = "dbugtest2016@gmail.com";
                //string fromPassword = "my@test#";

                if (email != null)
                {
                    try
                    {
                        int i;
                        for (i = 0; i <= email.Count; i++)
                        {

                            try
                            {
                                string str = "<font size='5'>Regards <br/> <b>DebugSoft</b></font>";
                                using (MailMessage mail = new MailMessage(/*from, email[i]*/))
                                {
                                    List<DateTime> days = db.Customer.Where(s => (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) < 0))
                  .Select(t => t.EndedDate).ToList();

                                    List<string> Name = db.Customer.Where(s => (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) < 0))
                  .Select(t => t.FirstName).ToList();
                                    mail.To.Add(new MailAddress(email[i]));
                                    mail.Subject = "Newspaper Date is being expired";
                                    //mail.Body = "Dear"+Name[i]+","
                                    //    +"Your Remaining NewsPaper Days is"+(days[i]-DateTime.Now).Days+"Days";

                                    //mail.Body = "<html><body><p>Dear " + Name[i] + ",</p><p>Your Remaining NewsPaper Days is " + (days[i] - DateTime.Now).Days + " Days.</p><p>Regards,<br>-DE!BUGSOFT</br></p></body></html>";

                                    mail.Body = "Dear" + " " + Name[i] + "," + "<Br/> Your Account was expired" + " " + (DateTime.Now - days[i]).Days + " " + " days ago .Please Contact our office for reactivation.<br/><br/>" + str;



                                    mail.IsBodyHtml = true;
                                    SmtpClient smtp = new SmtpClient();
                                    //smtp.Host = "smtp.gmail.com";
                                    //smtp.EnableSsl = true;
                                    //smtp.UseDefaultCredentials = false;
                                    //NetworkCredential networkCredential = new NetworkCredential(from, fromPassword);

                                    //smtp.Credentials = networkCredential;
                                    //smtp.Port = 587;
                                    smtp.Send(mail);

                                    // return View();
                                }

                            }
                            catch
                            {

                                ViewBag.Message = "Email sent to Expired Customer";
                            }
                        }
                    }
                    catch
                    {
                        ViewBag.ErrorMessage = "Email sending failed";
                    }
                    try
                    {
                        String Operation = "Mail Sent to Expired Customer";
                        db.ActivityLog.Add(new ActivityLog
                        {
                            Operation = Operation,
                            CreatedBy = Session["userEmail"].ToString(),
                            CreatedDate = DateTime.Now

                        });
                        db.SaveChanges();
                    }
                    catch
                    {
                        ViewBag.ErrorMessage = "Email sending failed";
                    }
                }
                return View();
            }
            else
            {
                int BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                List<string> email = db.Customer.Where(s => s.BranchId == BranchId && (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) < 0))
           .Select(t => t.Email).ToList();
                if (email == null)
                {
                    ViewBag.ErrorMessage = "Email sent to Expired Customer";
                }

                //string from = "dbugtest2016@gmail.com";
                //string fromPassword = "my@test#";

                if (email != null)
                {
                    try
                    {
                        int i;
                        for (i = 0; i <= email.Count; i++)
                        {

                            try
                            {
                                string str = "<font size='5'>Regards <br/> <b>DebugSoft</b></font>";
                                using (MailMessage mail = new MailMessage(/*from, email[i]*/))
                                {
                                    List<DateTime> days = db.Customer.Where(s => (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) < 0))
                  .Select(t => t.EndedDate).ToList();

                                    List<string> Name = db.Customer.Where(s => (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) < 0))
                  .Select(t => t.FirstName).ToList();
                                    mail.To.Add(new MailAddress(email[i]));
                                    mail.Subject = "Newspaper Date is being expired";
                                    //mail.Body = "Dear"+Name[i]+","
                                    //    +"Your Remaining NewsPaper Days is"+(days[i]-DateTime.Now).Days+"Days";

                                    //mail.Body = "<html><body><p>Dear " + Name[i] + ",</p><p>Your Remaining NewsPaper Days is " + (days[i] - DateTime.Now).Days + " Days.</p><p>Regards,<br>-DE!BUGSOFT</br></p></body></html>";

                                    mail.Body = "Dear" + " " + Name[i] + "," + "<Br/> Your Account was expired" + " " + (DateTime.Now - days[i]).Days + " " + " days ago .Please Contact our office for reactivation.<br/><br/>" + str;



                                    mail.IsBodyHtml = true;
                                    SmtpClient smtp = new SmtpClient();
                                    //smtp.Host = "smtp.gmail.com";
                                    //smtp.EnableSsl = true;
                                    //smtp.UseDefaultCredentials = false;
                                    //NetworkCredential networkCredential = new NetworkCredential(from, fromPassword);

                                    //smtp.Credentials = networkCredential;
                                    //smtp.Port = 587;
                                    smtp.Send(mail);

                                    // return View();
                                }

                            }
                            catch
                            {

                                ViewBag.Message = "Email sent to Expired Customer";
                            }
                        }
                    }
                    catch
                    {
                        ViewBag.ErrorMessage = "Email sending failed";
                    }
                    try
                    {
                        String Operation = "Mail Sent to Expired Customer";
                        db.ActivityLog.Add(new ActivityLog
                        {
                            BranchId = BranchId,
                            Operation = Operation,
                            CreatedBy = Session["userEmail"].ToString(),
                            CreatedDate = DateTime.Now

                        });
                        db.SaveChanges();
                    }
                    catch
                    {
                        ViewBag.ErrorMessage = "Email sending failed";
                    }
                }
                return View();
            }

        }



        public ActionResult DeadLineMail()
        {
            if (Session["Category"].ToString() == "SuperAdmin")
            {
                List<string> email = db.Customer.Where(s => (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) <= 10)).Where(s => (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) > 0))
           .Select(t => t.Email).ToList();


                //string from = "dbugtest2016@gmail.com";
                //string fromPassword = "my@test#";

                if (email != null)
                {
                    try
                    {
                        int i;
                        for (i = 0; i <= email.Count; i++)
                        {
                            try
                            {
                                string str = "<font size='5'>Regards <br/> <b>DebugSoft</b></font>";
                                using (MailMessage mail = new MailMessage(/*from, email[i]*/))
                                {
                                    List<DateTime> days = db.Customer.Where(s => (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) <= 10)).Where(s => (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) > 0))
                  .Select(t => t.EndedDate).ToList();

                                    List<string> Name = db.Customer.Where(s => (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) <= 10)).Where(s => (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) > 0))
                  .Select(t => t.FirstName).ToList();
                                    mail.To.Add(new MailAddress(email[i]));
                                    mail.Subject = "Newspaper Date is being expired";
                                    //mail.Body = "Dear"+Name[i]+","
                                    //    +"Your Remaining NewsPaper Days is"+(days[i]-DateTime.Now).Days+"Days";

                                    //mail.Body = "<html><body><p>Dear " + Name[i] + ",</p><p>Your Remaining NewsPaper Days is " + (days[i] - DateTime.Now).Days + " Days.</p><p>Regards,<br>-DE!BUGSOFT</br></p></body></html>";

                                    mail.Body = "Dear" + " " + Name[i] + "," + "<Br/> Your expiring date is near,Please renew in " + " " + (days[i] - DateTime.Now).Days + " " + "days for uninterupted subscription<br/><br/>" + str;



                                    mail.IsBodyHtml = true;
                                    SmtpClient smtp = new SmtpClient();
                                    //smtp.Host = "smtp.gmail.com";
                                    //smtp.EnableSsl = true;
                                    //smtp.UseDefaultCredentials = false;
                                    //NetworkCredential networkCredential = new NetworkCredential(from, fromPassword);

                                    //smtp.Credentials = networkCredential;
                                    //smtp.Port = 587;
                                    smtp.Send(mail);

                                }
                            }
                            catch
                            {
                                ViewBag.Message = "Mail sent to Deadline Customer";

                            }

                        }
                    }
                    catch
                    {

                        ViewBag.ErrorMessage = "Mail sending Failed";
                    }
                    try
                    {
                        String Operation = "Mail Sent to Deadline Customer";
                        db.ActivityLog.Add(new ActivityLog
                        {
                            Operation = Operation,
                            CreatedBy = Session["userEmail"].ToString(),
                            CreatedDate = DateTime.Now

                        });
                        db.SaveChanges();
                    }
                    catch
                    {
                        ViewBag.ErrorMessage = "Email sending failed";
                    }
                }
                return View();
            }




            else
            {
                int BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                List<string> email = db.Customer.Where(s =>s.BranchId == BranchId && (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) <= 10)).Where(s => (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) > 0))
                   .Select(t => t.Email).ToList();


                //string from = "dbugtest2016@gmail.com";
                //string fromPassword = "my@test#";

                if (email != null)
                {
                    try
                    {
                        int i;
                        for (i = 0; i <= email.Count; i++)
                        {
                            try
                            {
                                string str = "<font size='5'>Regards <br/> <b>DebugSoft</b></font>";
                                using (MailMessage mail = new MailMessage(/*from, email[i]*/))
                                {
                                    List<DateTime> days = db.Customer.Where(s => (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) <= 10)).Where(s => (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) > 0))
                  .Select(t => t.EndedDate).ToList();

                                    List<string> Name = db.Customer.Where(s => (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) <= 10)).Where(s => (DbFunctions.DiffDays(DateTime.Now, s.EndedDate) > 0))
                  .Select(t => t.FirstName).ToList();
                                    mail.To.Add(new MailAddress(email[i]));
                                    mail.Subject = "Newspaper Date is being expired";
                                    //mail.Body = "Dear"+Name[i]+","
                                    //    +"Your Remaining NewsPaper Days is"+(days[i]-DateTime.Now).Days+"Days";

                                    //mail.Body = "<html><body><p>Dear " + Name[i] + ",</p><p>Your Remaining NewsPaper Days is " + (days[i] - DateTime.Now).Days + " Days.</p><p>Regards,<br>-DE!BUGSOFT</br></p></body></html>";

                                    mail.Body = "Dear" + " " + Name[i] + "," + "<Br/> Your expiring date is near,Please renew in " + " " + (days[i] - DateTime.Now).Days + " " + "days for uninterupted subscription<br/><br/>" + str;



                                    mail.IsBodyHtml = true;
                                    SmtpClient smtp = new SmtpClient();
                                    //smtp.Host = "smtp.gmail.com";
                                    //smtp.EnableSsl = true;
                                    //smtp.UseDefaultCredentials = false;
                                    //NetworkCredential networkCredential = new NetworkCredential(from, fromPassword);

                                    //smtp.Credentials = networkCredential;
                                    //smtp.Port = 587;
                                    smtp.Send(mail);

                                }
                            }
                            catch
                            {
                                ViewBag.Message = "Mail sent to Deadline Customer";

                            }

                        }
                    }
                    catch
                    {

                        ViewBag.ErrorMessage = "Mail sending Failed";
                    }
                    try
                    {
                        int Branch = Convert.ToInt32(Session["BranchId"].ToString());
                        String Operation = "Mail Sent to Deadline Customer";
                        db.ActivityLog.Add(new ActivityLog
                        {
                            BranchId = Branch,
                            Operation = Operation,
                            CreatedBy = Session["userEmail"].ToString(),
                            CreatedDate = DateTime.Now

                        });
                        db.SaveChanges();
                    }
                    catch
                    {
                        ViewBag.ErrorMessage = "Email sending failed";
                    }
                }
                return View();
            }



        }

    }

}

