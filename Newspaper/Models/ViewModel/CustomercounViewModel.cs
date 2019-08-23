using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models.ViewModel
{
    public class CustomercounViewModel
    {
        public string CustomerId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName  { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string EndedDate { get; set; }
        public string NewspaperName { get; set; }
        public string DistributorName { get; set; }
        public DateTime PaperDispatchDate { get; set; }
        public string RegisterBy { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Amount { get; set; }
    }
}