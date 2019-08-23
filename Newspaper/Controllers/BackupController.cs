using Newspaper.Filters;
using Newspaper.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Newspaper.Controllers
{
    [SessionCheck(Role ="SuperAdmin,Supervisor")]
    public class BackupController : Controller
    {
        private activities log = new activities();

        // GET: Backup
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult backup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult backup(string directory)
        {
            string backupDIR = "";
            string drive = "";
           backupDIR = ConfigurationManager.AppSettings["Path"].ToString();
            if (!System.IO.Directory.Exists(backupDIR))
            {
                System.IO.Directory.CreateDirectory(backupDIR);
            }
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["Newspaper"].ConnectionString;
                SqlConnection conx = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conx;
                cmd.CommandType = CommandType.StoredProcedure;
                conx.Open();
                cmd = new SqlCommand("backup database Newspaper to disk='" + backupDIR + "\\" + DateTime.Now.ToString("ddMMyyyy") + ".Bak'", conx);
                cmd.ExecuteNonQuery();
                conx.Close();
                ViewBag.Message = "Backup Database file created successfully in 'Backup_Database_Newspaper' folder in " + drive + "  drive.";
                log.AddActivity("Data backup succesfully");
        //if (SendMail())
        //{
        //    ViewBag.Message = "Backup database successfully";
        //}
        //else
        //{
        //    ViewBag.Message = "Backup database successfully but backup file not send to mail.";
        //}
    }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error Occured During DB backup process !<br>" + ex.ToString();
                // lblError.Text = "Error Occured During DB backup process !<br>" + ex.ToString();
            }
            return View();
        }




    }
}