using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRC.Models
{
    public class Widget
    {
        public Guid Id { get; set; }
        public Func Name { get; set; }
        public Guid PageId { get; set; }
    }

    public enum Func
    {
        Slideshow,
        Event,
        Newsletter,
        Testimonial,
        Photo
    }
}
