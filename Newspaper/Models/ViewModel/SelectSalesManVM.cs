using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models.ViewModel
{
    public class SelectSalesManVM
    {
        public int CustomerId { get; set; }
        public int SalesManId { get; set; }
        public String SalesManName { get; set; }
        public string NewsPaperName { get; set; }
        public DateTime ReportDate { get; set; }
        public string NepDate { get; set; }
        public string branch { get; set; }
        public int Count { get; set; }
        public Service service { get; set;}
    

    }
}