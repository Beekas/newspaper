using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Newspaper.Models.ViewModel
{
    public class AdminViewModel
    {

        public int Id { get; set; }
        [Display(Name = "Employee Id")]
        public string EmployeeId { get; set; }
        [Display(Name="Full Name")]
        [Required(ErrorMessage = "FullName Required")]
        public string FullName { get; set; }
        
        [Required(ErrorMessage = "Email Required")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "Not a valid email")]

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Username Required")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }

        [Display(Name = "Phone No:")]
        [Required(ErrorMessage = "Phoneno: Required")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Work-Phone No:")]
        [DataType(DataType.PhoneNumber)]
        public string WorkPhone { get; set; }
        public string URL { get; set; }

        [Display(Name = "PP Size Photo")]
        public string PPSizePhoto { get; set; }
        [NotMapped]

        public HttpPostedFileBase ImageFile { get; set; }
        
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

     
    }
}