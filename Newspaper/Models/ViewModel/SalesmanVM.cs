using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models.ViewModel
{
    public class SalesmanVM
    {
        public SalesMan salesman { get; set; }
        public Customer customer { get; set; }
        public ServiceAssign serviceAssign { get; set; }
        public Service service { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Customertype { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Newspaper { get; set; }
        public string Quantity { get; set; }
        public DateTime Ended { get; set; }
        public string EndedDate { get; set; }
        public string Paperdispatchdate { get; set; }

    }
}