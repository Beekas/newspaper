using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Payoption { get; set; }
        public string BankName { get; set; }
        public string BankAcc { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime Paymentdate { get; set; }
        public string Nepalidate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public IEnumerable<Customer> customers { get; set; }
        public int CustomerId { get; set; }
        public IEnumerable<Newspaper> Newspapers { get; set; }
        public int NewspaperId { get; set; }
        public int BillNo { get; set; }
        public string FiscalYear { get; set; }
        public virtual FiscalYear fiscalyear { get; set; }
    }
}