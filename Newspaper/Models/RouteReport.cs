using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models
{
    public class RouteReport
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string NepaliDate { get; set; }
        public int gp_Quantity { get; set; }
        public int rn_Quantity { get; set; }
        public int muna_Quantity { get; set; }
        public int Yuwa_Quantity { get; set; }
        public int madhu_Quantity { get; set; }
        public int gp_total { get; set; }
        public int rn_total { get; set; }
        public int muna_total { get; set; }
        public int Yuwa_total { get; set; }
        public int madhu_total { get; set; }
        public int gp_grandtotal { get; set; }
        public int rn_grandtotal { get; set; }
        public int muna_grandtotal { get; set; }
        public int Yuwa_grandtotal { get; set; }
        public int madhu_grandtotal { get; set; }
        public virtual Route Route { get; set; }
        public int? RouteId { get; set; }
        public virtual GroupName Groupname { get; set; }
        public int? AgentId { get; set; }
    }
}