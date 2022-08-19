using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRC.Models.ViewModels
{
    public class NewsLetterEditViewModel
    {
        [Required]
        public Guid Id { get; set; }
        public IFormFile File { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Url { get; set; }
    }
}
