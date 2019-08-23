using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models.ViewModel
{
    public class AccountantVM
    {
        public Customer Customer { get; set; }
        public Service Service { get; set; }
        public SalesMan SalesMan { get; set; }
        public ServiceAssign ServiceAssign { get; set; }
        public Account Account { get; set; }
        public string Endeddate { get; set; }
    }
}