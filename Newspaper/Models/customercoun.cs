using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Newspaper.Models
{
    public class Customercoun
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Customer Id")]
        public string CustomerId { get; set; }

        [Required(ErrorMessage = "  NewsPapers Name Required")]
        [Display(Name = "NewsPaper Name")]
        public int? ServiceId { get; set; }

        [Required(ErrorMessage = "  SalesMan Name Required")]
        [Display(Name = "Distributor Name")]
        public int? SalesManId { get; set; }

        public int Quanity { get; set; } = 1;

        public int Amount { get; set; }


        [Column(TypeName = "Date")]
        public DateTime RegisterDate { get; set; }
        public string RegisteredBy { get; set; }

        [Display(Name = "Dispatch Date")]
        [Column(TypeName = "Date")]
        public DateTime PaperDispatchDate { get; set; }

        public string Duration { get; set; }

        [Column(TypeName = "Date")]
        [Display(Name = "Ended Date")]
        public DateTime EndedDate { get; set; }

        public string Status { get; set; }
        public string NepaliDate { get; set; }


        public virtual SalesMan SaleMan { get; set; }




        public ICollection<Service> Service { get; set; }

        public ICollection<Customer> Customers { get; set; }

    }
}
