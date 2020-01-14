using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models.ViewModel
{
    public class CounterVM
    {

        public Customer Customer { get; set; }
        public ServiceAssign ServiceAssign { get; set; }
        public Service NewsPaper { get; set; }
        public SalesMan SalesMan { get; set; }
        public string CustomerType { get; set; }
        public string Billdate { get; set; }
        public string NepaliDate { get; set; }
        public string Paperdispatch { get; set; }
        public int Quantity { get; set; }
        public string enddate { get; set; }
        public string Phone { get; set; }
        public int billNo { get; set; }
        public bool payment { get; set; } 
        public string fiscalYear { get; set; }
    }
}