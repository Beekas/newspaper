using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Newspaper.Filters
{
    public class SessionCheck : ActionFilterAttribute, IActionFilter
    {
        public string Role { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                HttpContext ctx = HttpContext.Current;
                if (HttpContext.Current.Session["id"] == null)
                {
                    filterContext.Result = new RedirectToRouteResult(
                     new RouteValueDictionary
                     {
                                { "controller", "Account" },
                                { "action", "Login" }
                     });
                }
                string[] roles = Role.Split(',');
                string val = HttpContext.Current.Session["Category"].ToString();
                int pos = Array.IndexOf(roles, val);
                if (pos > -1)
                {
                    base.OnActionExecuting(filterContext);
                }
                else
                {
                    filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                //    Boolean notfound = false;
                //if (roles.Length > 0)
                //{
                //    foreach (var item in roles)
                //    {
                //        if (HttpContext.Current.Session["Category"].ToString() == item)
                //        {
                //            //filterContext.Result = new RedirectResult("~/Account/Login");
                //            //return;
                           
                //            break;
                //        }
                //    }
                //    filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.NotFound);
                   

                //}


            }
            catch

            {
                filterContext.Result = new RedirectToRouteResult(
                   new RouteValueDictionary
                   {
                    { "controller", "Account" },
                    { "action", "Login" }
                   });
            }
        }
    }
}