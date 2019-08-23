using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models
{
    public class ServiceAssign
    {
        public int Id { get; set; }
        public DateTime PaperDispatchDate { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public string Duration { get; set; }
        public Decimal GrandTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime EndedDate { get; set; }
        public string NepaliDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool status { get; set; }
        public IEnumerable<Customer> customers { get; set; }
        public int CustomerId { get; set; }
        public IEnumerable<Newspaper> Newspapers { get; set; }
        public int NewspaperId { get; set; }
        public IEnumerable<SalesMan> salesMen { get; set; }
        public int SalesManId { get; set; }
        public string BillNo { get; set; }
    }
}