using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Newspaper.Models
{
    public class Branch
    {
       public int BranchId { get; set; }
        [Display(Name ="Branch Name")]
        public string BranchName { get; set; }
        [Display(Name = "Branch Address")]
        public string BranchAddress { get; set; }
        [Display(Name = "Branch Phone")]
        public string BrachPhone { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Admin> Admins { get; set; }
        public virtual ICollection<SalesMan> SalesMen{ get; set; }
    }
}