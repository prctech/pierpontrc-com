using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRC.Models.ViewModels
{
    public class PageEditViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        [RegularExpression(@"^[a-z\-]{1,40}$",
         ErrorMessage = "Only lower case and dashes are allowed.")]
        public string Url { get; set; }
        [Required]
        [Display(Name = "Has Menu?")]
        public bool SubPages { get; set; }
        public Guid MenuId { get; set; }
        public List<Menu> Menus { get; set; }
        public Guid ParentPage { get; set; }
    }
}
