using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Newspaper.Models
{
    public class Admin
    {
        public int Id { get; set; }

      
        [Display(Name="Employee Id")]
        public string EmployeeId { get; set; }
        [Required(ErrorMessage = "  Full Name Required")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "E-Mail Required")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "User Name Required")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password Required")]
        
        public string Password { get; set; }
         
        [Display(Name="Phone No:")]
        public string Phone { get; set; }
         [Required(ErrorMessage = "Category Required")]
        
         public string Category { get; set; }
        public string WorkPhone { get; set; }
        public string URL { get; set; }
        public string PPSizePhoto { get; set; }
        public bool Status { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
         [Column(TypeName = "Date")]
        public DateTime CreatedDate { get; set; }
         [Column(TypeName = "Date")]
         public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string randompass { get; set; }

        public virtual Branch Branch { get; set; }
        public int? BranchId { get; set; }
    }
}