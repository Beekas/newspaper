using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models.ViewModel
{
    public class DistributorVM
    {
        public string DistributorName { get; set; }
        public int Quantity { get; set; }
        public int Added { get; set; }
        public int Subs { get; set; }
        public int Total { get; set; }
        public string newspaperName { get; set; }
        public string branch { get; set; }
        public string  NepaliDate { get; set; }
    }
}