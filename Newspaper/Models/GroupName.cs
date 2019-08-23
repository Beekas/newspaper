using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models
{
    public class GroupName
    {
        public int Id { get; set; }
       
        public string AgentName { get; set; }
        public string Pan { get; set; }
        public string GP_Quantity { get; set; }
        public string RN_Quantity { get; set; }
        public string MUNA_Quantity { get; set; }
        public string Madhu_Quantity { get; set; }
        public string Yuwa_Quantity { get; set; }
        public string Address { get; set; }
        public string Time { get; set; }
        public string RouteName { get; set; }
        public string Transport { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string URL { get; set; }

        public virtual ICollection<Prakashanreport> Prakashanreports { get; set; }

        public virtual ICollection<RouteReport> RouteReports { get; set; }

        public virtual Route Route { get; set; }
        public int? RouteId { get; set; }
    }
}