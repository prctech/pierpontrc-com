using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRC.Models.ViewModels
{
    public class PageViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public bool HasSubPages { get; set; }
        public bool HasContact { get; set; }
        public Guid MenuId { get; set; }
        public Guid ParentPage { get; set; }
        public List<Page> SubPages { get; set; }
    }
}
