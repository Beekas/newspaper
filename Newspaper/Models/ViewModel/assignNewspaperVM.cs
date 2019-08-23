using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Newspaper.Models.ViewModel
{
    public class assignNewspaperVM
    {
        public int Id { get; set; }
        public DateTime PaperDispatchDate { get; set;}
        public DateTime EndedDate { get; set; }
        public string endDate { get; set; }
        public string dispatch { get; set; }
        public string phone { get; set; }
        public string NepaliDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Newspapername { get; set; }
        
        public string CusId { get; set; }
        public bool status { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Paper { get; set; }
        public string Distributor { get; set; }
        public int Quantity { get; set; }
        public int CustomerId { get; set; }
        public int SalesManId { get; set; }
        public SelectList Newspapers { get; set; }
        public SelectList Salesmans { get; set; }
        public IEnumerable<Service> Newspaper { get; set; }
        public IEnumerable<SalesMan> Salesman { get; set; }
        public IEnumerable< Customer> Customer { get; set; }

        public int NewspaperId { get; set; }
        public int  Duration { get; set; }
        public string custId { get; set; }
    }
}