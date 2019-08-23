using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models.ViewModel
{
    public class AddedCustomerVM
    {
        public string CustomerId { get; set; }
        public string  NewsPaperName { get; set; }
        public DateTime ReportDate{ get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string MPhone { get; set; }
        public DateTime PaperDispatchDate { get; set; }
        public string NepaliDate { get; set; }
        public DateTime EndedDate { get; set; }
        public string enddate { get; set; }
        public string dispatch { get; set; }
        public string SalesMan { get; set; }
        public string branch { get; set; }
    }
}