using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRC.Models
{
    public class Page
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public Guid ParentPage { get; set; }
        public Guid MenuId { get; set; }
        public bool SubPages { get; set; }
        public bool Contact { get; set; }
    }
}
