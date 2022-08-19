using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRC.Models.ViewModels
{
    public class PageCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Url { get; set; }
        [Display(Name = "Parent Page")]
        public List<Page> Pages { get; set; }
        public bool ParentPage { get; set; }
        [Required]
        [Display(Name = "Has Menu?")]
        public bool HasSubPages { get; set; }
        public Menu Menu { get; set; }
        public List<Menu> Menus { get; set; }
    }
}
