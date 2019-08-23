using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newspaper.Models
{
    public class PrakashanGroup
    {
        public int Id { get; set; }
        public string GroupId { get; set; }
        public string  GroupName { get; set; }
        //public virtual ICollection<GroupName> GroupNames { get; set; }
    }
}