using Newspaper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Newspaper.Controllers
{
    public class CustomercounController : Controller
    {
        // GET: Customercoun
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(Customercoun counter)
        {

            return View();
        }
    }
}