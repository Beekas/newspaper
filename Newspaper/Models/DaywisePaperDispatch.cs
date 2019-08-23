using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models
{
    public class DaywisePaperDispatch
    {
        public int Id { get; set; }

        //public int SalesmanId { get; set; }
     
        //public int ServiceId { get; set; }

        //public int CustomerId { get; set; }

        public DateTime PaperDispatchDate { get; set; }

        public string Remarks { get; set; }

        public string SubmittedBy { get; set; }

        public DateTime SubmittedDate { get; set; }

        public virtual SalesMan SaleMan { get; set; }
        public int? SalesManId { get; set; }

        public virtual Service Service { get; set; }
        public int? ServiceId { get; set; }

        public virtual Customer Customer { get; set; }
        public int? CustomerId { get; set; }

    }
}