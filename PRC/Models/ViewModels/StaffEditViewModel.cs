using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRC.Models.ViewModels
{
    public class StaffEditViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Department { get; set; }
        public List<Department> Departments { get; set; }
        public string Role { get; set; }
        [Required]
        [Display(Name = "Department Head?")]
        public bool Lead { get; set; }
        public string Image { get; set; }
    }
}
