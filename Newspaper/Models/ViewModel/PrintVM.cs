using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models.ViewModel
{
    public class PrintVM
    {
        public string CustomerId { get; set; }
        public string  CustomerName { get; set; }
        public string NepaliDate { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Subject { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal GrandTotal { get; set; }
        public string AmountWord { get; set; }
        public string BankName { get; set; }
        public string BankAcc { get; set; }
        public string StartDate { get; set; }
        public string fiscalyear { get; set; }
        public string EndedDate { get; set; }
        public int Quantity { get; set; }
        public string Remarks { get; set; }
        public string NewspaperName { get; set; }
        public string BillNo { get; set; }

        public IEnumerable<GetPrintVM> GetPrint { get; set; }

    }
    public class GetPrintVM {
        public int BillNo { get; set; }
        public string CustomerName { get; set; }
        public string NepaliDate { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int Quantity { get; set; }
    }
}