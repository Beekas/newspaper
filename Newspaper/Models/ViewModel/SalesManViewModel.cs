using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Newspaper.Models.ViewModel
{
    public class SalesManViewModel
    {
        public int Id { get; set; }
         [Display(Name = "Sales Man Id")]
        public int? SalesManId { get; set; }
        [Display(Name="Full Name")]
        [Required(ErrorMessage = "Full Name Required")]
        public string FullName { get; set; }
         [Display(Name = "E-Mail")]
        [Required(ErrorMessage = "Email Required")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "Not a valid email")]
        public string Email { get; set; }
         [Display(Name = "Alt E-mail")]
        public string AltEmail { get; set; }
        
        [Display(Name = "Phone 1")]
        [Required(ErrorMessage = "Phone No: Required")]
        [DataType(DataType.PhoneNumber)]
        public long Phone1 { get; set; }
         
        [Display(Name = "Phone 2")]
        public long Phone2 { get; set; }
    }
}