using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models
{
    public class Prakashanreport
    {
        public int Id { get; set; }
      
        public DateTime Date { get; set; }
        public string NepaliDate { get; set; }

        public int gp_Total { get; set; }
        public int rn_Total { get; set; }
        public string Remarks { get; set; }

        public virtual PrakashanGroup PrakashanGroup { get; set; }
        public int? GroupId { get; set; }
    }
}