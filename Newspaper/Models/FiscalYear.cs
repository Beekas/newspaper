using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models
{
    public class FiscalYear
    {
        public int Id { get; set; }
        public string NepYear { get; set; }
        public string EngYear { get; set; }
        public bool Status { get; set; }
        public string CreatedBy { get; set; }
        public string EditedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime EditedDate { get; set; }

    }
}