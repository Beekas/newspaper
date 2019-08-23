using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models.ViewModel
{
    public class ProductionReportvm
    {
        public string datetoday { get; set; }
        public string dateyes { get; set; }
        public int gptoday { get; set; }
        public int gpyes { get; set; }
        public int gpdiff { get; set; }
        public int gptodaytotal { get; set; }
        public int gpyestotal { get; set; }
        public int gpdifftotal { get; set; }
        public int rntoday { get; set; }
        public int rnyes { get; set; }
        public int rndiff { get; set; }
        public int rntodaytotal { get; set; }
        public int rnyestotal { get; set; }
        public int rndifftotal { get; set; }
        public string groupname { get; set; }

    }
}