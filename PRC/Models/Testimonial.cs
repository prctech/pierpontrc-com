using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRC.Models
{
    public class Testimonial
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Member { get; set; }
        public string Content { get; set; }
    }
}
