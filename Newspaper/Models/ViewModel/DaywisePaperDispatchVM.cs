using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models.ViewModel
{
    public class DaywisePaperDispatchVM
    {
        public int Id { get; set; }

        public DateTime PaperDispatchDate { get; set; }

        public string Remarks { get; set; }

        public string SubmittedBy { get; set; }

        public DateTime SubmittedDate { get; set; }

        public int? SalesManId { get; set; }
        
        public int? ServiceId { get; set; }

        public List<CustomerVM> CustomerVM { get; set; }

    }
}