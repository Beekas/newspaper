using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Newspaper.Models.ViewModel
{
    public class CustomerViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Customer Id")]
        public string CustomerId { get; set; }

        [Display(Name = "SalesMan Name")]
        public int? SalesManId { get; set; }

        [Display(Name = "NewsPaper Name")]
        public int? ServiceId { get; set; }

        [Display(Name = "First Name")]

         [Required(ErrorMessage = "  First Name Required")]
        public string FirstName { get; set; }
         [Display(Name = "Middle Name")]

        public string MiddleName { get; set; }

         [Display(Name = "Last Name")]
         [Required(ErrorMessage = "  Last Name Required")]
        public string LastName { get; set; }

         [Display(Name = "Mobile Number")]
         [Required(ErrorMessage = "  Mobile Number Required")]
         [DataType(DataType.PhoneNumber)]
        public long MPhone { get; set; }

         [Display(Name = "E-Mail")]
         [Required(ErrorMessage = "  Email Required")]
         [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "Not a valid email")]
        public string Email { get; set; }
         
        [Display(Name = "Alt E-mail")]
        public string AltEmail { get; set; }


         [Display(Name = "Home No:")]
        [DataType(DataType.PhoneNumber)]
        public string HomeNo { get; set; }
        public string Tole { get; set; }
        public string Address { get; set; }

         [Required(ErrorMessage = "  Provience Required")]
        public string Provience { get; set; }

         [Display(Name = "GPRS Latitude")]
        public Nullable<decimal> Gprslatitude { get; set; }

         [Display(Name = "GPRS Longitude")]
        public Nullable<decimal> GprsLongitude { get; set; }
        public string URL { get; set; }

         [Display(Name = "Customer Info")]
        public String CustomerInfo { get; set; }

         [Display(Name = "Customer Type")]
        public string CustomerType { get; set; }

       [Display(Name = "Org./Agen./Staff/Comp./Addprov.")]
        public String TypeDetail { get; set; }

       public String Amount { get; set; }
        public Nullable<System.DateTime> RegisterDate { get; set; }
        public string RegisteredBy { get; set; }

         [Display(Name = "Paper Dispatch Date")]
        public DateTime PaperDispatchDate { get; set; }
        public string Duration { get; set; }

        [Display(Name="Ended Date")]
        public int EndedDate { get; set; }

        public string Status { get; set; }

        
    }
}