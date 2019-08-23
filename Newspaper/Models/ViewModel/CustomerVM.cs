using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models.ViewModel
{
    public class CustomerVM
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string NewsPaperName { get; set; }
        public string SalesMan { get; set; }
        public string NepaliDate { get; set; }
        public bool IsPaperDispatched { get; set; }
    }
}