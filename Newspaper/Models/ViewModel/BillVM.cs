using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Newspaper.Models.ViewModel
{
    public class BillVM
    {
        public string BillNo { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string PrintedDate { get; set; }
        public string NewspaperName { get; set; }
        public string Address { get; set; }
        public int AccountId { get; set; }
        public string fiscalyear { get; set; }
        public SelectList Newspapers { get; set; }
        public SelectList BillNos { get; set; }
    }
}