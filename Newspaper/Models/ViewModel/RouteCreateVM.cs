using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models.ViewModel
{
    public class RouteCreateVM
    {
        public int? routeId { get; set; }
        public List<GroupName> groupNames { get; set; }
    }
}