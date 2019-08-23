using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Newspaper.Models
{
    [Table("tblService")]
    public class Service
    {
        public int Id { get; set; }
        
        [Display(Name="Service Code")]
        public string  ServiceCode { get; set; }
        [Required(ErrorMessage="NewsPaper Name Required")]
        [Display(Name = "Newspaper Name")]
        public string NewsPaperName { get; set; }
        [Display(Name = "Time Base")]
        public string TimeBase { get; set; }
        public string CreatedBy { get; set; }
        public string EditedBy { get; set; }
        public string Images { get; set; }
        
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        [Column(TypeName = "Date")]
        public DateTime EditedDate { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }

        public virtual Service Services { get; set; }

        public virtual ICollection<DaywisePaperDispatch> DaywisePaperDispatch { get; set; }
        
    }
}