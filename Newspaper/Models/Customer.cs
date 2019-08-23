using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Newspaper.Models
{
    [Table("tblCustomer")]
    public class Customer
    {
        

        public int Id { get; set; }

        [Display(Name = "Customer Id")]
        public string CustomerId { get; set; }



        [Display(Name = "First Name")]
       

        public string FirstName { get; set; }
       

        //public string MiddleName { get; set; }

        //[Display(Name = "Last Name")]
        
        //public string LastName { get; set; }

        //[Display(Name = "Mobile Number")]
       
        //[DataType(DataType.PhoneNumber)]
        public string MPhone { get; set; }



        [Display(Name = "E-Mail")]
        //[Required(ErrorMessage = "  Email Required")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "Not a valid email")]
        public string Email { get; set; }

        [Display(Name = "Alt E-mail")]
        public string AltEmail { get; set; }


        [Display(Name = "Home No:")]

        public string HomeNo { get; set; }
        public string Tole { get; set; }
       
        public string Address { get; set; }

        //[Required(ErrorMessage = "  Provience Required")]
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

       
        

      
        public DateTime RegisterDate { get; set; }
        public string RegisteredBy { get; set; }
        
        //[Display(Name = "Dispatch Date")]
        //[Column(TypeName = "Date")]
        //public DateTime PaperDispatchDate { get; set; }
        //public string Duration { get; set; }
        // [Column(TypeName = "Date")]
        //[Display(Name = "Ended Date")]
        //public DateTime EndedDate { get; set; }

        //public string Status { get; set; }
        //public string NepaliDate { get; set; }


        //public virtual SalesMan SaleMan { get; set; }

        //[Required(ErrorMessage = "  SalesMan Name Required")]
        //[Display(Name = "Distributor Name")]
        //public int? SalesManId { get; set; }


        //public virtual Service Service { get; set; }

        //[Required(ErrorMessage = "  NewsPapers Name Required")]
        //[Display(Name = "NewsPaper Name")]
        //public int? ServiceId { get; set; }

        public virtual Branch Branch { get; set; }
        public int? BranchId { get; set; }

        //public virtual ICollection<DaywisePaperDispatch> DaywisePaperDispatch { get; set; }

       
    }
}