using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Newspaper.Models.ViewModel
{
    public class MailViewModel
    {
        public string To { get; set; }
        public string MailSubject { get; set; }
        [AllowHtml]
        public string MailBody { get; set; }
      
    }
}