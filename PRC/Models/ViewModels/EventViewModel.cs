using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRC.Models.ViewModels
{
    public class EventViewModel
    {
        [Required]
        public string MonthYear { get; set; }
    }
}
