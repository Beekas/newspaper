using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Newspaper.Models.ViewModel
{
      [Table("tblAreaRegister")]
    public class AreaRegister
    {
      
        public int Id { get; set; }
        public virtual SalesMan SaleMan { get; set; }

       
        [Display(Name="SalesMan")]
       
        public int? SalesManId { get; set; }

         
        public string Area { get; set; }
          [AllowHtml]
        public String Comment { get; set; }
           [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
           [Display(Name = "Edited By")]
        public string EditedBy { get; set; }
           [Display(Name = "MOdified By")]
        public string ModifiedBy { get; set; }
    }
}