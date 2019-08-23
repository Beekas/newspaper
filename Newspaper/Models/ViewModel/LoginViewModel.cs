using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Newspaper.Models.ViewModel
{
    public class LoginViewModel
    {
        public string EmployeeId { get; set; }
        
        [Required(ErrorMessage = "Username Required")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Password Required")]

        public string Password { get; set; }
        public bool RememberMe { get; set; }

    }
}