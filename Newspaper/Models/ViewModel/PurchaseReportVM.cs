using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Newspaper.Models.ViewModel
{
    public class PurchaseReportVM
    {
        public int Edit_ID { get; set; }
        public string NepaliDate { get; set; }
        public int gp_Total { get; set; }
        public int rn_Total { get; set; }
        public string Remarks { get; set; }
        public string NewspaperName { get; set; }
        public int Id { get; set; }
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        //public List<PrakashanGroup> Group { get; set; }
        public DateTime Date { get; set; }
        
    }
}