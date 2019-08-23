using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models
{
    public class HoldingDays
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
      
        public DateTime HoldingStartDays { get; set; }
        public DateTime HoldingEndDays { get; set; }
        public string RemainingHoldingDays { get; set; }
    }
}