using Newspaper.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Newspaper.Models
{
     [Table("tblSalesMan")]
    public class SalesMan
    {
        public int Id { get; set; }

        
         [Display(Name="Sales Man Id")]
        public string SalesManId { get; set; }
         [Required(ErrorMessage = "Full Name Required")]
          [Display(Name = "Full Name")]
        public string FullName { get; set; }
         
          [Display(Name = "E-Mail")]
        public string Email { get; set; }
          [Display(Name = "Alt Email")]
        public string AltEmail { get; set; }

        
          [Display(Name = "Phone 1:")]
        public string Phone1 { get; set; }
          [Display(Name = "Phone 2:")]
        public string Phone2 { get; set; }

        public virtual Branch Branch { get; set; }
        public int? BranchId { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<AreaRegister> AreaRegisters { get; set; }

        public virtual ICollection<DaywisePaperDispatch> DaywisePaperDispatch { get; set; }
    }
}