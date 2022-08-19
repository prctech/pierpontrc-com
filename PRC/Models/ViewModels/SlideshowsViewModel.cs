using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRC.Models.ViewModels
{
    public class SlideshowsViewModel
    {
        public IFormFile File { get; set; }
        [Required]
        public string Url { get; set; }
    }
}
