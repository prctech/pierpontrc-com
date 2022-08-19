using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRC.Models
{
    public class Slideshow
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
