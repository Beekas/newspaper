using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models
{
    public class Route
    {
        public int Id { get; set; }
        public string RouteId { get; set; }
        public string RouteName { get; set; }
        public virtual ICollection<GroupName> GroupNames { get; set; }
        public virtual ICollection<RouteReport> RouteReports { get; set; }
    }
}